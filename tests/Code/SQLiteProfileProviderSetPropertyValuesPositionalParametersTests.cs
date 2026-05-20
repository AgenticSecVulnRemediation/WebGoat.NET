using Xunit;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Web.Profile;
using System.Configuration;

// Assumption: source namespace is TechInfoSystems.Data.SQLite based on file content.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_SetPropertyValues_UsesPositionalParametersTests
    {
        [Fact]
        public void SetPropertyValues_UserLookupQuery_UsesPositionalParameters_PreventsInjectionViaUsername()
        {
            // Arrange
            // The fix changed:
            //  SELECT UserId ... WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId
            // to positional parameters "?" with AddWithValue(null, ...)
            // Regression test: a malicious username containing SQL metacharacters should not break query.

            // Create in-memory db with minimal tables.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE aspnet_Applications (ApplicationId TEXT, ApplicationName TEXT, Description TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE [aspnet_Users] (UserId TEXT, Username TEXT, LoweredUsername TEXT, ApplicationId TEXT, IsAnonymous INTEGER, LastActivityDate TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE [aspnet_Profile] (UserId TEXT, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB, LastUpdatedDate TEXT);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO aspnet_Applications (ApplicationId, ApplicationName, Description) VALUES ('memApp','mem','');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_Users] (UserId, Username, LoweredUsername, ApplicationId, IsAnonymous, LastActivityDate) VALUES ('u1','bob','bob','memApp',0,'2020-01-01');";
                cmd.ExecuteNonQuery();
            }

            // Force provider to use our in-memory connection by setting private static _connectionString.
            var type = typeof(SQLiteProfileProvider);
            type.GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, cn.ConnectionString);
            type.GetField("_membershipApplicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, "memApp");
            type.GetField("_applicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, "profileApp");

            var provider = new SQLiteProfileProvider();

            // Build a SettingsContext and one property value.
            var ctx = new SettingsContext();
            ctx.Add("UserName", "bob' OR 1=1 --");
            ctx.Add("IsAuthenticated", true);

            var properties = new SettingsPropertyValueCollection();
            var prop = new SettingsProperty("p1")
            {
                PropertyType = typeof(string),
                Provider = provider,
                SerializeAs = SettingsSerializeAs.String,
                IsReadOnly = false,
                DefaultValue = ""
            };
            var spv = new SettingsPropertyValue(prop)
            {
                PropertyValue = "v1",
                IsDirty = true
            };
            properties.Add(spv);

            // Act
            // Should not throw; because user doesn't exist with that lowered username, provider returns early.
            provider.SetPropertyValues(ctx, properties);

            // Assert
            // Ensure no profile row was written (since userIsAuthenticated=true and no user id matched).
            using var verify = cn.CreateCommand();
            verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile];";
            var count = Convert.ToInt32(verify.ExecuteScalar());
            Assert.Equal(0, count);

            // Also verify the real user still exists (injection did not delete/alter table).
            verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users] WHERE UserId='u1';";
            Assert.Equal(1, Convert.ToInt32(verify.ExecuteScalar()));
        }
    }
}
