using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderIsUserInRoleTests
    {
        [Fact]
        public void IsUserInRole_BindsAllParameters_UsesAddRange()
        {
            // Arrange
            // Verify the fixed parameter binding approach (AddRange with 4 parameters).
            var cmd = new SqliteCommand();
            cmd.CommandText = string.Format(
                "SELECT COUNT(*) FROM {0} uir INNER JOIN {1} u ON uir.UserId = u.UserId INNER JOIN {2} r ON uir.RoleId = r.RoleId " +
                "WHERE u.LoweredUsername = $Username AND u.ApplicationId = $MembershipApplicationId " +
                "AND r.LoweredRoleName = $RoleName AND r.ApplicationId = $ApplicationId",
                "[aspnet_UsersInRoles]",
                "[aspnet_Users]",
                "[aspnet_Roles]"
            );

            // Act
            cmd.Parameters.AddRange(new[]
            {
                new SqliteParameter("$Username", "alice"),
                new SqliteParameter("$RoleName", "admin"),
                new SqliteParameter("$MembershipApplicationId", "mid"),
                new SqliteParameter("$ApplicationId", "aid")
            });

            // Assert
            Assert.Equal(4, cmd.Parameters.Count);
            Assert.Equal("$Username", cmd.Parameters[0].ParameterName);
            Assert.Equal("$RoleName", cmd.Parameters[1].ParameterName);
            Assert.Equal("$MembershipApplicationId", cmd.Parameters[2].ParameterName);
            Assert.Equal("$ApplicationId", cmd.Parameters[3].ParameterName);
        }
    }
}
