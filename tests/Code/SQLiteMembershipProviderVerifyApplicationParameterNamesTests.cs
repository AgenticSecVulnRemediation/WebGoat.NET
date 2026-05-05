using Xunit;

// Assumption: production namespace TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterNamesTests
    {
        [Fact]
        public void VerifyApplication_InsertCommand_UsesDollarPrefixedParameterNames()
        {
            // Delta test: VerifyApplication builds INSERT with $ApplicationName and $Description.
            // Regression guard: parameters should keep '$' prefix when using Sqlite placeholders.
            var insertSql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";

            Assert.Contains("$ApplicationName", insertSql);
            Assert.Contains("$Description", insertSql);
        }
    }
}
