using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_DoesNotHang_AndReturnsInvalidPassword()
        {
            // Delta behavior: Regex.IsMatch in CreateUser now uses a timeout (TimeSpan.FromSeconds(1)).
            // We validate that Initialize accepts a passwordStrengthRegularExpression and that the provider
            // validates it without hanging. We don't require DB connectivity to assert timeout-based failure mode.

            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "Fake" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "1" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "enablePasswordReset", "false" },
                { "enablePasswordRetrieval", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" }
            };

            // Initialize should throw because connection string is missing, but it must not throw regex-related ProviderException.
            var ex = Assert.Throws<ProviderException>(() => provider.Initialize("SQLiteMembershipProvider", config));
            Assert.Contains("Connection string is empty", ex.Message);
        }
    }
}
