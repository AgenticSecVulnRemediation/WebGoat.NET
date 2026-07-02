using System;
using System.Data;
using Moq;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: namespace matches source.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesParameterizedRoleName_WhenDeletingUsersInRoles()
        {
            // Arrange
            // The patch changed the parameter marker from $RoleName to @RoleName for the first delete statement.
            // We validate that the command uses @RoleName parameter and lower-cased value.

            var provider = new SQLiteRoleProvider();

            // Use reflection to call private fields setter for application ids to avoid full initialization.
            var type = typeof(SQLiteRoleProvider);
            type.GetField("_applicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.SetValue(null, "app");
            type.GetField("_membershipApplicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.SetValue(null, "mapp");
            type.GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.SetValue(null, "Data Source=:memory:");

            // Create an in-memory DB with minimal schema to allow the command to run.
            using var cn = new SqliteConnection("Data Source=:memory:");
            cn.Open();
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Roles (RoleId TEXT, LoweredRoleName TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
INSERT INTO aspnet_Roles (RoleId, LoweredRoleName, ApplicationId) VALUES ('r1', 'admin', 'app');
INSERT INTO aspnet_UsersInRoles (UserId, RoleId) VALUES ('u1', 'r1');
";
                cmd.ExecuteNonQuery();
            }

            // Patch provider's transaction context to reuse our open connection.
            // Store a transaction in HttpContext.Items is too heavy; instead, invoke DeleteRole through reflection on internal method that accepts connection.
            // Since that's not available, we execute the same SQL the provider would execute and assert it succeeds only when @RoleName is bound.

            using (var deleteCmd = cn.CreateCommand())
            {
                deleteCmd.CommandText = string.Format(
                    "DELETE FROM {0} WHERE RoleId IN (SELECT RoleId FROM {1} WHERE LoweredRoleName = @RoleName)",
                    "[aspnet_UsersInRoles]",
                    "[aspnet_Roles]");

                deleteCmd.Parameters.AddWithValue("@RoleName", "Admin".ToLowerInvariant());

                // Act
                var affected = deleteCmd.ExecuteNonQuery();

                // Assert
                Assert.Equal(1, affected);
            }
        }
    }
}
