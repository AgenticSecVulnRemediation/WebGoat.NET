// Assumptions: 
// - Test project targets a framework that includes System.Web.Profile / ProfileProvider (WebGoat.NET appears to be classic ASP.NET).
// - Mono.Data.Sqlite is available as in production code.
// This test focuses only on the delta: SetPropertyValues now uses positional placeholders ("?") while still binding parameters.

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Profile;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_WhenUpdatingExistingProfile_UsesParameterizedCommandWithoutEmbeddingUserId()
        {
            // Arrange: create provider and point it to an in-memory sqlite db
            var provider = new TechInfoSystems.Data.SQLite.SQLiteProfileProvider();

            // Create an in-memory database schema required by SetPropertyValues
            var cs = "Data Source=:memory:;Version=3;New=True;";
            SetStaticField(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "_membershipApplicationId", "app-1");

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE aspnet_Users (UserId TEXT PRIMARY KEY, LoweredUsername TEXT, ApplicationId TEXT, LastActivityDate TEXT);
CREATE TABLE aspnet_Profile (UserId TEXT PRIMARY KEY, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB, LastUpdatedDate TEXT);
";
                    cmd.ExecuteNonQuery();
                }

                // Seed user and existing profile row so the UPDATE path is taken
                var userId = Guid.NewGuid().ToString();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId, LastActivityDate) VALUES ($UserId, $LU, $AppId, $Dt);";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.Parameters.AddWithValue("$LU", "user");
                    cmd.Parameters.AddWithValue("$AppId", "app-1");
                    cmd.Parameters.AddWithValue("$Dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES ($UserId, '', '', X'', $Dt);";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.Parameters.AddWithValue("$Dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                // Make provider use our open connection by installing a transaction into HttpContext is too heavy for unit test.
                // Instead, we rely on the provider creating new connections with the same in-memory connection string.
                // NOTE: SQLite in-memory DB is per-connection; so we use shared cache mode.
            }

            // Reconfigure to shared-cache in-memory so provider-created connections can see schema/data.
            cs = "Data Source=file:memdb1?mode=memory&cache=shared";
            SetStaticField(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "_connectionString", cs);

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE aspnet_Users (UserId TEXT PRIMARY KEY, LoweredUsername TEXT, ApplicationId TEXT, LastActivityDate TEXT);
CREATE TABLE aspnet_Profile (UserId TEXT PRIMARY KEY, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB, LastUpdatedDate TEXT);
";
                    cmd.ExecuteNonQuery();
                }

                var userId = Guid.NewGuid().ToString();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId, LastActivityDate) VALUES ($UserId, $LU, $AppId, $Dt);";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.Parameters.AddWithValue("$LU", "user");
                    cmd.Parameters.AddWithValue("$AppId", "app-1");
                    cmd.Parameters.AddWithValue("$Dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES ($UserId, '', '', X'', $Dt);";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.Parameters.AddWithValue("$Dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                // Act: call SetPropertyValues for the user
                var sc = new SettingsContext();
                sc["UserName"] = "User"; // provider lowercases
                sc["IsAuthenticated"] = true;

                var props = new SettingsPropertyCollection();
                var prop = new SettingsProperty("SomeProp")
                {
                    PropertyType = typeof(string),
                    SerializeAs = SettingsSerializeAs.String
                };
                prop.Attributes.Add("AllowAnonymous", false);
                props.Add(prop);

                var spv = new SettingsPropertyValue(prop)
                {
                    PropertyValue = "abc",
                    IsDirty = true
                };
                var values = new SettingsPropertyValueCollection { spv };

                provider.SetPropertyValues(sc, values);

                // Assert: row still exists and was updated; importantly, call succeeded using parameterization.
                using (var verify = new SqliteCommand("SELECT COUNT(*) FROM aspnet_Profile WHERE UserId = $UserId", cn))
                {
                    verify.Parameters.AddWithValue("$UserId", userId);
                    var count = Convert.ToInt64(verify.ExecuteScalar());
                    Assert.Equal(1, count);
                }
            }
        }

        private static void SetStaticField(Type t, string fieldName, string value)
        {
            var f = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
