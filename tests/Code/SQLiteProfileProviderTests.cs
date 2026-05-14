// Assumptions:
// - Source namespace matches file folder: TechInfoSystems.Data.SQLite
// - This test executes against an in-memory SQLite DB using Mono.Data.Sqlite.
// - It uses reflection to call private static DeleteProfile to validate the parameter marker fix.

using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesAtParameters_DeletesExistingProfile()
        {
            // Arrange
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                // Minimal schema required by DeleteProfile
                cmd.CommandText = @"
CREATE TABLE aspnet_Users (
  UserId TEXT PRIMARY KEY,
  LoweredUsername TEXT NOT NULL,
  ApplicationId TEXT NOT NULL
);
CREATE TABLE aspnet_Profile (
  UserId TEXT PRIMARY KEY,
  PropertyNames TEXT,
  PropertyValuesString TEXT,
  PropertyValuesBinary BLOB
);
";
                cmd.ExecuteNonQuery();
            }

            var userId = Guid.NewGuid().ToString();
            var username = "Bob";
            var lowered = username.ToLowerInvariant();
            var appId = "app-1";

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId) VALUES (@uid, @lu, @app)";
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@lu", lowered);
                cmd.Parameters.AddWithValue("@app", appId);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary) VALUES (@uid, '', '', X'')";
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
            }

            // Set private static field _membershipApplicationId so DeleteProfile matches row.
            var providerType = typeof(SQLiteProfileProvider);
            providerType.GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, appId);

            var deleteProfile = providerType.GetMethod(
                "DeleteProfile",
                BindingFlags.NonPublic | BindingFlags.Static,
                binder: null,
                types: new[] { typeof(SqliteConnection), typeof(SqliteTransaction), typeof(string) },
                modifiers: null);

            Assert.NotNull(deleteProfile);

            // Act
            var deleted = (bool)deleteProfile!.Invoke(null, new object?[] { conn, null, username })!;

            // Assert
            Assert.True(deleted);

            using (var verify = conn.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM aspnet_Profile WHERE UserId = @uid";
                verify.Parameters.AddWithValue("@uid", userId);
                var remaining = Convert.ToInt32(verify.ExecuteScalar());
                Assert.Equal(0, remaining);
            }
        }
    }
}
