using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Code/SQLiteRoleProvider.cs".
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterMarker_ForApplicationId()
        {
            // Patch changed ApplicationId parameter marker from named "$ApplicationId" to positional "?".
            const string sql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = ?";

            Assert.Contains("ApplicationId = ?", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
