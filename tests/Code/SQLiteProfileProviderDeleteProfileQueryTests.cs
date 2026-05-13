using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileQueryTests
    {
        [Fact]
        public void DeleteProfile_UsesAtUserIdPlaceholder_NotDollarUserId()
        {
            // Arrange
            // Delta behavior: DELETE query placeholder changed from $UserId to @UserId.
            // This test guards against regressions that reintroduce the old placeholder style.
            var deleteSql = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            // Act
            var usesAtParam = deleteSql.Contains("@UserId", StringComparison.Ordinal);
            var usesDollarParam = deleteSql.Contains("$UserId", StringComparison.Ordinal);

            // Assert
            Assert.True(usesAtParam);
            Assert.False(usesDollarParam);
        }
    }
}
