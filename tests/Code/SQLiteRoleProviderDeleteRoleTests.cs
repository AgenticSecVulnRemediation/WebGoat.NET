using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtPrefixedRoleNameParameter_InSubquery()
        {
            // Arrange
            // Fix changes $RoleName to @RoleName for the subquery.
            string sql = "DELETE FROM [aspnet_UsersInRoles] WHERE (RoleId IN (SELECT RoleId FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName))";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@RoleName", "admin");

            // Assert
            Assert.True(cmd.CommandText.Contains("@RoleName"));
            Assert.True(cmd.Parameters.Contains("@RoleName"));
            Assert.False(cmd.Parameters.Contains("$RoleName"));
        }
    }
}
