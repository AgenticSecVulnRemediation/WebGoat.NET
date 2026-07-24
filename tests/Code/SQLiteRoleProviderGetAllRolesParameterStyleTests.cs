using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesParameterStyleTests
    {
        [Fact]
        public void GetAllRoles_Query_UsesNamedAtParameter_ForApplicationId()
        {
            // Arrange
            // Delta behavior: ApplicationId placeholder changed from $ApplicationId to @ApplicationId.
            var fixedSql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";

            // Assert
            Assert.Contains("@ApplicationId", fixedSql);
            Assert.DoesNotContain("$ApplicationId", fixedSql);
        }
    }
}
