using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Profile;
using Moq;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesNamedParametersWithAtPrefix_DoesNotThrowWhenMembershipApplicationIdContainsSqlMetaChars()
        {
            // Arrange
            // This test is a delta test for the change from "$Username/$ApplicationId" to "@Username/@ApplicationId"
            // in the SetPropertyValues() UserId lookup query.
            // We validate the secure behavior by ensuring the query still executes successfully even when
            // ApplicationId contains characters that would be problematic if concatenated into SQL.

            // Create provider instance without calling Initialize() (which requires config). We'll set private fields via reflection.
            var provider = new SQLiteProfileProvider();

            // Provide a temporary SQLite DB file.
            var dbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wg_profile_" + Guid.NewGuid() + ".db");
            var cs = $"Data Source={dbPath};Version=3";
            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE aspnet_Users (
  UserId TEXT PRIMARY KEY,
  Username TEXT,
  LoweredUsername TEXT,
  ApplicationId TEXT,
  LastActivityDate TEXT
);
CREATE TABLE aspnet_Profile (
  UserId TEXT PRIMARY KEY,
  PropertyNames TEXT,
  PropertyValuesString TEXT,
  PropertyValuesBinary BLOB,
  LastUpdatedDate TEXT
);
";
                    cmd.ExecuteNonQuery();
                }

                // Insert a user row with a "weird" application id.
                var appId = "app'; DROP TABLE aspnet_Users;--";
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Users(UserId, Username, LoweredUsername, ApplicationId, LastActivityDate) VALUES (@uid, @u, @lu, @app, @dt)";
                    cmd.Parameters.AddWithValue("@uid", "user-1");
                    cmd.Parameters.AddWithValue("@u", "Bob");
                    cmd.Parameters.AddWithValue("@lu", "bob");
                    cmd.Parameters.AddWithValue("@app", appId);
                    cmd.Parameters.AddWithValue("@dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                // Put provider connection string + membership app id into private statics.
                typeof(SQLiteProfileProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, cs);
                typeof(SQLiteProfileProvider).GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, appId);

                // Prepare settings context and one profile property.
                var sc = new SettingsContext();
                sc["UserName"] = "Bob";
                sc["IsAuthenticated"] = true;

                var props = new SettingsPropertyValueCollection();
                var sp = new SettingsProperty("Color")
                {
                    PropertyType = typeof(string),
                    Provider = provider,
                    SerializeAs = SettingsSerializeAs.String
                };
                var spv = new SettingsPropertyValue(sp)
                {
                    PropertyValue = "blue",
                    IsDirty = true
                };
                props.Add(spv);

                // Act + Assert
                // Should not throw; if the query was concatenated, the appId would break it.
                var ex = Record.Exception(() => provider.SetPropertyValues(sc, props));
                Assert.Null(ex);

                // And user table should still exist (no injection executed).
                using (var verifyCmd = cn.CreateCommand())
                {
                    verifyCmd.CommandText = "SELECT COUNT(*) FROM aspnet_Users";
                    var count = Convert.ToInt32(verifyCmd.ExecuteScalar());
                    Assert.True(count >= 1);
                }
            }

            // Cleanup
            try { System.IO.File.Delete(dbPath); } catch { /* ignore */ }
        }
    }
}
