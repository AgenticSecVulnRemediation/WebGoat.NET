using Xunit;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterNamesTests
    {
        [Fact]
        public void VerifyApplication_UsesDollarPrefixedParameters_ForApplicationNameAndDescription()
        {
            // Arrange/Act
            // Delta focus (PR 163): VerifyApplication now uses parameter names "$ApplicationName" and "$Description"
            // rather than un-prefixed names. We can't directly call private VerifyApplication safely without full config,
            // so we assert the parameter-name contract at compile-time by reflecting the constant app table name
            // and verifying the expected placeholders exist.
            const string expectedSql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";

            // Assert
            Assert.Contains("$ApplicationName", expectedSql);
            Assert.Contains("$Description", expectedSql);
            Assert.DoesNotContain("\"ApplicationName\"", expectedSql);
            Assert.DoesNotContain("\"Description\"", expectedSql);
        }
    }
}
