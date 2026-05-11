using Xunit;

// Note: Namespace inference is based on file path; adjust if the production assembly uses a different root namespace.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesNamedParameter_WithAtPrefix()
        {
            // Arrange
            // Security-relevant delta: SQL changed from $ApplicationId to @ApplicationId
            // We assert the expected query fragment uses '@'.
            var sql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";

            // Act / Assert
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
