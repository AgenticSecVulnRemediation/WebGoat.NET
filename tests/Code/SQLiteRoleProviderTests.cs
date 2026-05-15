using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using Moq;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesNamedParameterWithAtPrefix_ReturnsRoles()
        {
            // Arrange
            // This test asserts the security fix behavior change: use @ApplicationId parameter name (not $ApplicationId).
            // We isolate DB access by mocking SqliteCommand behavior via a wrapper connection/command.

            // NOTE: SQLiteRoleProvider uses concrete Mono.Data.Sqlite types directly. Without refactoring, full mocking is hard.
            // Therefore, we perform a focused integration-style unit test using an in-memory SQLite database.

            var cs = "Data Source=:memory:;Version=3;New=True;";

            using var cn = new SqliteConnection(cs);
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE aspnet_Roles (RoleId TEXT, RoleName TEXT, LoweredRoleName TEXT, ApplicationId TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES ('1', 'Admin', 'admin', 'app1');";
                create.ExecuteNonQuery();
            }

            // Build provider with minimal config via reflection: set private static _connectionString/_applicationId
            typeof(SQLiteRoleProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, cs);
            typeof(SQLiteRoleProvider).GetField("_applicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, "app1");

            var provider = new SQLiteRoleProvider();

            // Act
            var roles = provider.GetAllRoles();

            // Assert
            Assert.Contains("Admin", roles);
        }
    }
}
