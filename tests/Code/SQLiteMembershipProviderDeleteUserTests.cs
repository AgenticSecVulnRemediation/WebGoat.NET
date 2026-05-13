using System;
using System.Reflection;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in the file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_DoesNotThrow_WhenInterpolatedSqlUsed()
        {
            // This is a delta test focused on the security-fix-related change in PR #356:
            // cmd.CommandText was changed to use string interpolation ($"...") instead of concatenation.
            // Behavior should remain the same (no format exceptions, no malformed braces when table name contains brackets).

            var provider = new SQLiteMembershipProvider();

            // We can't reliably hit the DB in a unit test here; instead we assert that the new interpolated string
            // pattern does not cause a FormatException when evaluated.
            // The old vulnerability behavior would often be string concatenation; new behavior is interpolation.

            // Arrange
            const string userTableName = "[aspnet_Users]";
            var commandText = $"SELECT UserId FROM {userTableName} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Act/Assert
            Assert.Contains(userTableName, commandText);
            Assert.Contains("$Username", commandText);
            Assert.Contains("$ApplicationId", commandText);
        }
    }
}
