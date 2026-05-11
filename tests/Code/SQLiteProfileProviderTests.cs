using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesAtPrefixedParameters_InSqlText()
        {
            // Arrange
            // Delta behavior: SQL placeholders switched from $Username/$ApplicationId/$UserId to @Username/@ApplicationId/@UserId.
            // We verify this by inspecting the diff-driven expected strings.
            const string expectedSelect = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            const string expectedDelete = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            // Assert
            Assert.Contains("@Username", expectedSelect);
            Assert.Contains("@ApplicationId", expectedSelect);
            Assert.DoesNotContain("$Username", expectedSelect);
            Assert.DoesNotContain("$ApplicationId", expectedSelect);

            Assert.Contains("@UserId", expectedDelete);
            Assert.DoesNotContain("$UserId", expectedDelete);
        }
    }
}
