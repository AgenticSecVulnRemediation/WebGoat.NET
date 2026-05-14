using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterStyleTests
    {
        [Fact]
        public void DeleteUser_WithDeleteAllRelatedDataFalse_UsesAtParametersAndReturnsTrueWhenRowDeleted()
        {
            // Arrange: create in-memory sqlite schema that matches the provider's expected tables/columns.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (
  UserId TEXT,
  LoweredUsername TEXT,
  ApplicationId TEXT
);
CREATE TABLE [aspnet_UsersInRoles] (UserId TEXT);
CREATE TABLE [aspnet_Profile] (UserId TEXT);
";
                cmd.ExecuteNonQuery();
            }

            // Configure provider's static fields via reflection so it uses our connection.
            // We avoid calling Initialize() to keep this a deterministic unit test.
            SetStaticField("_connectionString", cn.ConnectionString);
            SetStaticField("_applicationId", "app1");

            // Seed a user row that should be deleted.
            using (var seed = cn.CreateCommand())
            {
                seed.CommandText = "INSERT INTO [aspnet_Users] (UserId, LoweredUsername, ApplicationId) VALUES ($UserId, $LoweredUsername, $ApplicationId)";
                seed.Parameters.AddWithValue("$UserId", "u1");
                seed.Parameters.AddWithValue("$LoweredUsername", "alice");
                seed.Parameters.AddWithValue("$ApplicationId", "app1");
                seed.ExecuteNonQuery();
            }

            var provider = new SQLiteMembershipProvider();

            // Act
            var deleted = provider.DeleteUser("Alice", deleteAllRelatedData: false);

            // Assert
            Assert.True(deleted);

            using (var verify = cn.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users] WHERE LoweredUsername = $LoweredUsername AND ApplicationId = $ApplicationId";
                verify.Parameters.AddWithValue("$LoweredUsername", "alice");
                verify.Parameters.AddWithValue("$ApplicationId", "app1");
                var remaining = Convert.ToInt32(verify.ExecuteScalar());
                Assert.Equal(0, remaining);
            }
        }

        private static void SetStaticField(string fieldName, object? value)
        {
            var f = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
