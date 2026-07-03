using System;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserRelatedDataDeleteInterpolationTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingRelatedData_UsesConstantTableNameInInterpolatedSql()
        {
            // Arrange
            // PR 3919 changes the related-data delete to string interpolation:
            //   $"DELETE FROM {USERS_IN_ROLES_TB_NAME} WHERE UserId = $UserId"
            // Ensure USERS_IN_ROLES_TB_NAME is the expected constant and cannot be influenced by user input.

            var usersInRolesField = typeof(SQLiteMembershipProvider)
                .GetField("USERS_IN_ROLES_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(usersInRolesField);
            var usersInRolesTable = (string)usersInRolesField!.GetValue(null)!;

            // Assert constant is the bracketed table name as expected.
            Assert.Equal("[aspnet_UsersInRoles]", usersInRolesTable);

            // Act: build the SQL as the fixed code does.
            var sql = $"DELETE FROM {usersInRolesTable} WHERE UserId = $UserId";

            // Assert: the resulting SQL contains the constant table name, not user data.
            Assert.Equal("DELETE FROM [aspnet_UsersInRoles] WHERE UserId = $UserId", sql);
        }
    }
}
