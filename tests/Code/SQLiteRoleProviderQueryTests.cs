using Xunit;

namespace OWASP.WebGoat.NET.Tests.Code
{
    public class SQLiteRoleProviderQueryTests
    {
        [Fact]
        public void GetAllRoles_Query_UsesPositionalParameterMarker()
        {
            // Arrange
            // Delta check: the provider switched from "$ApplicationId" to positional parameter "?".
            const string commandText = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = ?";

            // Assert
            Assert.Contains("ApplicationId = ?", commandText);
            Assert.DoesNotContain("$ApplicationId", commandText);
        }
    }
}
