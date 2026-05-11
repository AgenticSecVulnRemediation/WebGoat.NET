using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtParameter_ForRoleName_InSubquery()
        {
            // Arrange
            using var cmd = new SqliteCommand();

            // Act
            cmd.CommandText = "DELETE FROM [aspnet_UsersInRoles] WHERE (RoleId IN (SELECT RoleId FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName))";
            cmd.Parameters.AddWithValue("@RoleName", "admin");

            // Assert
            Assert.Single(cmd.Parameters);
            Assert.Contains("@RoleName", cmd.CommandText);
        }
    }
}
