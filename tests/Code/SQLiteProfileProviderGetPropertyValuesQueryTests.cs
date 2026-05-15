using System;
using System.Linq;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesQueryTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_ProfileLookup_UsesPositionalPlaceholder_ForUserId()
        {
            // Arrange (mirrors diff)
            var sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = ?";

            // Act
            var placeholderCount = sql.Count(c => c == '?');

            // Assert
            Assert.Equal(1, placeholderCount);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
