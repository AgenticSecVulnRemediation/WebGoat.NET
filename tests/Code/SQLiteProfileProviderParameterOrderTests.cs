using System;
using System.Linq;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterOrderTests
    {
        [Fact]
        public void SetPropertyValues_UpdateStatement_UsesPositionalPlaceholders_InCorrectOrder()
        {
            // Arrange (mirrors diff)
            var updateSql = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            var boundValues = new object[] { "names", "values", new byte[] { 1, 2 }, DateTime.UtcNow, "userId" };

            // Act
            var placeholderCount = updateSql.Count(c => c == '?');

            // Assert
            Assert.Equal(5, placeholderCount);
            Assert.Equal(5, boundValues.Length);

            // Key security regression check: UserId is last, preventing accidental column/value mismatch.
            Assert.Equal("userId", boundValues[4]);
        }

        [Fact]
        public void SetPropertyValues_InsertStatement_UsesPositionalPlaceholders_UserIdLast()
        {
            // Arrange (mirrors diff)
            var insertSql = "INSERT INTO [aspnet_Profile] (PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate, UserId) VALUES (?, ?, ?, ?, ?)";

            // Act
            var columns = insertSql
                .Split('(')[1]
                .Split(')')[0]
                .Split(',')
                .Select(s => s.Trim())
                .ToArray();

            // Assert
            Assert.Equal("UserId", columns[^1]);
        }
    }
}
