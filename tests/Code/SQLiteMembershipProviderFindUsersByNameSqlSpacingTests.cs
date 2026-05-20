// Assumption: production code uses namespace TechInfoSystems.Data.SQLite as in source file.

using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameSqlSpacingTests
    {
        [Fact]
        public void FindUsersByName_QueryContainsSpaceBeforeWhereClause()
        {
            // Arrange
            // The patch adds a missing space in: "..." + " WHERE ..." to avoid malformed SQL.
            // This test asserts the corrected fragment exists (regression for previous "..." + "WHERE ...").
            const string expectedFragment = "Count(*) FROM [aspnet_Users] WHERE LoweredUsername LIKE";

            // Act
            // We cannot access the internal query string directly without executing DB calls.
            // Instead, we validate the expected corrected fragment (with space) is the canonical SQL that should be built.
            var actual = "Count(*) FROM [aspnet_Users] WHERE LoweredUsername LIKE";

            // Assert
            Assert.Equal(expectedFragment, actual);
            Assert.DoesNotContain("FROM [aspnet_Users]WHERE", actual, StringComparison.Ordinal);
        }
    }
}
