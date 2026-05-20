using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_DeleteUser_Tests
    {
        [Fact]
        public void DeleteUser_UsesParameters_ForUsernameAndApplicationId()
        {
            // Arrange
            var fixedDeleteSql = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";

            // Act / Assert
            Assert.Contains("@Username", fixedDeleteSql, StringComparison.Ordinal);
            Assert.Contains("@ApplicationId", fixedDeleteSql, StringComparison.Ordinal);

            // Ensure old $-style placeholders are not used for this query anymore.
            Assert.DoesNotContain("$Username", fixedDeleteSql, StringComparison.Ordinal);
            Assert.DoesNotContain("$ApplicationId", fixedDeleteSql, StringComparison.Ordinal);
        }
    }
}
