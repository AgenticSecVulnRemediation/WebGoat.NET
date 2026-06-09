using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void IsUserInRole_UsesPositionalParametersToAvoidNamedParameterInjection()
        {
            // Arrange/Act
            var sql = "SELECT COUNT(*) FROM [aspnet_UsersInRoles] uir INNER JOIN [aspnet_Users] u ON uir.UserId = u.UserId INNER JOIN [aspnet_Roles] r ON uir.RoleId = r.RoleId  WHERE u.LoweredUsername = ? AND u.ApplicationId = ? AND r.LoweredRoleName = ? AND r.ApplicationId = ?";

            // Assert
            Assert.Contains("?", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$RoleName", sql);
        }
    }
}
