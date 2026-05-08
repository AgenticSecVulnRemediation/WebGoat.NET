using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta: placeholder tokens changed from $Username/$ApplicationId to @Username/@ApplicationId.
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_QueryUsesAtPlaceholders_NotDollarPlaceholders()
        {
            // Act
            var sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            // Assert
            Assert.Contains("@Username", sql);
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
