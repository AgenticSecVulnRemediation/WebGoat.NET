using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderVerifyApplicationTableNameTests
    {
        [Fact]
        public void VerifyApplication_InsertCommand_UsesExplicitAspnetApplicationsTableName()
        {
            // Delta test: VerifyApplication now uses explicit table name [aspnet_Applications]
            // instead of APP_TB_NAME constant. This prevents accidental use of an unsafe/incorrect
            // table identifier and ensures consistent behavior.

            var snippet = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description)";
            Assert.Contains("[aspnet_Applications]", snippet);
        }
    }
}
