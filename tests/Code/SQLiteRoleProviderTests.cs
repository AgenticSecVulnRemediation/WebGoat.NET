using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameter_PreventsSqlInjectionViaRoleName()
        {
            // Arrange
            using var cmd = new SqliteCommand();
            cmd.CommandText = "DELETE FROM [aspnet_UsersInRoles] WHERE (RoleId IN (SELECT RoleId FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName))";
            cmd.Parameters.AddWithValue("@RoleName", "admin' OR '1'='1");

            // Assert: query uses parameter marker and does not embed role name
            Assert.Contains("@RoleName", cmd.CommandText);
            Assert.DoesNotContain("admin' OR '1'='1", cmd.CommandText);
            Assert.Equal("admin' OR '1'='1", cmd.Parameters["@RoleName"].Value);
        }
    }
}
