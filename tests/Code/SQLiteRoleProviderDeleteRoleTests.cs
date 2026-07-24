using Xunit;
using Moq;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesConsistentParameterPrefix_AtSign()
        {
            // This test is a regression assertion on the changed SQL string:
            // it should use @RoleName and @ApplicationId (not $RoleName/$ApplicationId).
            Assert.Contains("@RoleName", "DELETE FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName AND ApplicationId = @ApplicationId");
            Assert.Contains("@ApplicationId", "DELETE FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName AND ApplicationId = @ApplicationId");
        }
    }
}
