using System;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfiles_ByUsernames_DeletesUsingParameter_DoesNotThrowOnSqlMetaCharsInUserId()
        {
            // Arrange
            // Delta test for change in DeleteProfile(): WHERE UserId = $UserId -> WHERE UserId = @UserId.
            // We validate that delete works even if the stored userId contains SQL metacharacters.

            var provider = new SQLiteProfileProvider();

            var dbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wg_profile_del_" + Guid.NewGuid() + ".db");
            var cs = $"Data Source={dbPath};Version=3";

            var appId = "app-1";
            var username = "Bob";
            var lowered = "bob";
            var userId = "id'; DELETE FROM aspnet_Profile;--";

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

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Users(UserId, Username, LoweredUsername, ApplicationId, LastActivityDate) VALUES (@uid,@u,@lu,@app,@dt)";
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@lu", lowered);
                    cmd.Parameters.AddWithValue("@app", appId);
                    cmd.Parameters.AddWithValue("@dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (@uid,'','','',@dt)";
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                typeof(SQLiteProfileProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, cs);
                typeof(SQLiteProfileProvider).GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, appId);

                // Act
                var deleted = provider.DeleteProfiles(new[] { username });

                // Assert
                Assert.Equal(1, deleted);

                using (var verify = cn.CreateCommand())
                {
                    verify.CommandText = "SELECT COUNT(*) FROM aspnet_Profile";
                    var remainingProfiles = Convert.ToInt32(verify.ExecuteScalar());
                    Assert.Equal(0, remainingProfiles);
                }

                using (var verify = cn.CreateCommand())
                {
                    verify.CommandText = "SELECT COUNT(*) FROM aspnet_Users";
                    var remainingUsers = Convert.ToInt32(verify.ExecuteScalar());
                    Assert.Equal(1, remainingUsers); // DeleteProfile only deletes from Profile table
                }
            }

            try { System.IO.File.Delete(dbPath); } catch { /* ignore */ }
        }
    }
}
