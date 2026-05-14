using System;
using System.Collections.Specialized;
using System.Configuration;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeoutToMitigateReDoS()
        {
            // Delta behavior: Regex.IsMatch(..., timeout: 1s) should throw RegexMatchTimeoutException
            // for catastrophic patterns instead of hanging.

            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "TestSqlite" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "1" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "requiresUniqueEmail", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "enablePasswordReset", "false" },
                { "enablePasswordRetrieval", "false" }
            };

            // Ensure a connection string exists; provider will attempt to read it during Initialize.
            // This test focuses on regex timeout; DB is not used because exception occurs before DB calls.
            if (ConfigurationManager.ConnectionStrings["TestSqlite"] == null)
            {
                // In some test runners, ConnectionStrings is read-only; if so, skip deterministically.
                throw new SkipException("Test requires ConfigurationManager.ConnectionStrings['TestSqlite'] to be configured for unit tests.");
            }

            provider.Initialize("SQLiteMembershipProvider", config);

            var password = new string('a', 20000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                provider.CreateUser("user1", password, "e@e", null, null, true, null, out _));
        }
    }
}
