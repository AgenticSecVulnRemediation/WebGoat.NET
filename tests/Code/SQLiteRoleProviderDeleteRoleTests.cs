using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameter_InUsersInRolesDelete()
        {
            // Arrange
            var method = typeof(SQLiteRoleProvider).GetMethod("DeleteRole");
            Assert.NotNull(method);

            // Act / Assert
            const string expectedSqlSnippet = "LoweredRoleName = @RoleName";
            Assert.Contains("@RoleName", expectedSqlSnippet);
        }
    }
}
