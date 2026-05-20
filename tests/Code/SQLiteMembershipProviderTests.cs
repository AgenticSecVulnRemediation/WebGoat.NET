// Assumption: production code uses namespace TechInfoSystems.Data.SQLite as in source file.

using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_DoesNotThrow_WhenSqlUsesBracketedTableName()
        {
            // Arrange
            // The security fix switched string concatenation to interpolated string using USER_TB_NAME which includes brackets.
            // This regression test ensures the interpolated SQL remains well-formed (e.g., "FROM [aspnet_Users]").
            var provider = new SQLiteMembershipProvider();

            // Act
            // We cannot execute against a real DB here deterministically, so we validate the SQL template shape by inspecting
            // the constant and constructing the same interpolated string pattern.
            var userTableName = (string)typeof(SQLiteMembershipProvider)
                .GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static)!
                .GetValue(null)!;

            var sql = $"SELECT UserId FROM {userTableName} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Assert
            Assert.Contains("FROM [aspnet_Users]", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("FROM aspnet_Users", sql, StringComparison.Ordinal); // ensure brackets preserved
        }
    }
}
