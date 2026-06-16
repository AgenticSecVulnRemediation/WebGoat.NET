using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleParameterMarkerTests
    {
        [Fact]
        public void DeleteRole_UsesAtParameters_ForRoleNameAndApplicationId()
        {
            // Regression test for security fix switching $RoleName/$ApplicationId to @RoleName/@ApplicationId.
            var roleProviderType = typeof(TechInfoSystems.Data.SQLite.SQLiteRoleProvider);
            var assemblyText = roleProviderType.Assembly.ToString();

            Assert.Contains("@RoleName", assemblyText);
            Assert.Contains("@ApplicationId", assemblyText);
            Assert.DoesNotContain("$RoleName\"", assemblyText);
            Assert.DoesNotContain("$ApplicationId\"", assemblyText);
        }
    }
}
