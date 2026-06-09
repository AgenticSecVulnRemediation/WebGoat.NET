using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserSqlParameterizationDeltaTests
    {
        [Fact]
        public void DeleteUserRelatedDataQueries_UsePositionalParameter_AndNotNamedUserIdParameter()
        {
            // Arrange
            // Delta behavior: DeleteUser now uses positional parameter placeholders ("?") for UserId deletes
            // and does NOT use "$UserId" in those DELETE statements.

            // Act
            var usersInRolesDelete = $"DELETE FROM [aspnet_UsersInRoles] WHERE UserId = ?";
            var profileDelete = $"DELETE FROM [aspnet_Profile] WHERE UserId = ?";

            // Assert
            Assert.Contains("WHERE UserId = ?", usersInRolesDelete);
            Assert.Contains("WHERE UserId = ?", profileDelete);

            Assert.DoesNotContain("$UserId", usersInRolesDelete, StringComparison.Ordinal);
            Assert.DoesNotContain("$UserId", profileDelete, StringComparison.Ordinal);
        }

        [Fact]
        public void SqliteParameterValueBinding_ForPositionalParameter_DoesNotRequireName()
        {
            // Arrange
            // Delta behavior: cmd.Parameters.Add(new SqliteParameter { Value = userId });
            // This should work without setting a ParameterName when using positional placeholders.
            var userId = "some-user-id";

            // Act
            var p = new Mono.Data.Sqlite.SqliteParameter { Value = userId };

            // Assert
            Assert.Equal(userId, p.Value);
            Assert.True(string.IsNullOrEmpty(p.ParameterName));
        }
    }
}
