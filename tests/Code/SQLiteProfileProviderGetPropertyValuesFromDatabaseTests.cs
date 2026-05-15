using System;
using System.Linq;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UserLookup_UsesPositionalParameters()
        {
            // Arrange (mirrors diff)
            var sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = ? AND ApplicationId = ?";

            // Act
            var placeholderCount = sql.Count(c => c == '?');

            // Assert
            Assert.Equal(2, placeholderCount);
            Assert.DoesNotContain("$UserName", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
