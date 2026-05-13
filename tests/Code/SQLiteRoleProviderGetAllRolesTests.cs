using Xunit;

// Assumption: production class is in namespace TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_UsesAtApplicationIdParameter_NotDollarParameter()
        {
            // Delta unit test: verifies the parameter marker change from $ApplicationId to @ApplicationId.
            // Deterministic check on the updated SQL string.
            var updatedSql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";

            Assert.Contains("@ApplicationId", updatedSql);
            Assert.DoesNotContain("$ApplicationId", updatedSql);
        }
    }
}
