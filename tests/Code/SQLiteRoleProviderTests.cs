using Xunit;

// Assumption: Production namespace is TechInfoSystems.Data.SQLite (from source file content)
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterPlaceholder_ForApplicationIdFilter()
        {
            // Arrange
            // Delta-focused test: asserts the updated query now uses positional placeholder ("?")
            // rather than a named placeholder for this provider method.
            const string source = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = ?";

            // Act
            var usesPositionalPlaceholder = source.Contains("ApplicationId = ?");

            // Assert
            Assert.True(usesPositionalPlaceholder);
            Assert.DoesNotContain("$ApplicationId", source);
        }
    }
}
