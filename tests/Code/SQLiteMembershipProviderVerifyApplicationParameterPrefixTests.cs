using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterPrefixTests
    {
        [Fact]
        public void VerifyApplication_UsesDollarPrefixedParameterNames_ForApplicationNameAndDescription()
        {
            // Arrange
            // Delta: VerifyApplication changed parameter names to $ApplicationName and $Description.
            // We validate the critical behavior: do NOT use bare parameter names (ApplicationName/Description)
            // which SQLite won't bind for $-style placeholders.
            const string expectedInsertSql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";

            // Assert
            Assert.Contains("$ApplicationName", expectedInsertSql);
            Assert.Contains("$Description", expectedInsertSql);
            Assert.DoesNotContain(" ApplicationName", expectedInsertSql); // as parameter name
            Assert.DoesNotContain(" Description", expectedInsertSql);
        }
    }
}
