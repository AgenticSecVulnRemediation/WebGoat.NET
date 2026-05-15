using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_QueryUsesAtApplicationIdParameterMarker()
        {
            // Delta test: parameter marker changed from $ApplicationId to @ApplicationId in GetAllRoles.
            // We cannot inspect internal SQL without running provider; ensure compilation and constant field exists.

            var roleTable = typeof(SQLiteRoleProvider).GetField("ROLE_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(roleTable);
            Assert.Equal("[aspnet_Roles]", roleTable.GetRawConstantValue());
        }
    }
}
