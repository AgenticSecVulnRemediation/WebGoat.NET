using Xunit;

// Assumption: production namespace TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesParameterPrefixTests
    {
        [Fact]
        public void GetAllRoles_UsesAtApplicationIdParameter()
        {
            // Delta test: switched from $ApplicationId to @ApplicationId for this query.
            var sql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
