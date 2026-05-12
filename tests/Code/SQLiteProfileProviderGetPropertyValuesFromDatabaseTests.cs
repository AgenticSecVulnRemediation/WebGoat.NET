// Assumptions: test project references Mono.Data.Sqlite and System.Web.Profile.
// Delta: query now uses "@UserId" and string.Format with PROFILE_TB_NAME.

using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Profile;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UserIdParameter_IsBoundWithAtUserId()
        {
            // Arrange
            var provider = new TechInfoSystems.Data.SQLite.SQLiteProfileProvider();
            var cs = "Data Source=file:memdb2?mode=memory&cache=shared";
            SetStaticField(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "_membershipApplicationId", "app-1");

            using var cn = new SqliteConnection(cs);
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
                cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES ($UserId, 'X:S:0:1:', 'a', X'', $Dt);";
                cmd.Parameters.AddWithValue("$UserId", userId);
                cmd.Parameters.AddWithValue("$Dt", DateTime.UtcNow.ToString("o"));
                cmd.ExecuteNonQuery();
            }

            // Act: calling GetPropertyValues triggers GetPropertyValuesFromDatabase internally.
            var sc = new SettingsContext();
            sc["UserName"] = "User";

            var properties = new SettingsPropertyCollection();
            var prop = new SettingsProperty("X") { PropertyType = typeof(string), SerializeAs = SettingsSerializeAs.String };
            properties.Add(prop);

            var result = provider.GetPropertyValues(sc, properties);

            // Assert: value came back (meaning SELECT executed successfully with parameter binding).
            Assert.NotNull(result["X"]);
            Assert.Equal("a", result["X"].PropertyValue);
        }

        private static void SetStaticField(Type t, string fieldName, string value)
        {
            var f = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
