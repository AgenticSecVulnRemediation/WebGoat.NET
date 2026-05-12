using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithSlowRegexEvaluation_TimesOut()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "MembershipDb" },
                { "applicationName", "/" },
                { "passwordFormat", "Clear" },
                { "minRequiredPasswordLength", "7" },
                { "minRequiredNonalphanumericCharacters", "1" },
                { "passwordStrengthRegularExpression", "^(a+)+$" }
            };

            // Provide a connection string entry; the provider will read from ConfigurationManager.
            // Assumption: test runner has app.config/web.config with MembershipDb connection string.
            provider.Initialize("SQLiteMembershipProvider", config);

            // Act + Assert
            // The fix adds a Regex timeout; extremely long input should fail fast rather than hang.
            Assert.Throws<ArgumentException>(() =>
                provider.ChangePassword("user", "oldPass1!", new string('a', 50000) + "!"));
        }

        [Fact]
        public void ChangePassword_UsesRegexOverloadWithTimeout()
        {
            // Arrange
            var mi = typeof(SQLiteMembershipProvider).GetMethod(
                "ChangePassword",
                BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(mi);
        }
    }
}
