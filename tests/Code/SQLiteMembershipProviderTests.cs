using System;
using System.Collections.Specialized;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingRegex_ThrowsDueToRegexTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "1" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "enablePasswordReset", "true" },
                { "enablePasswordRetrieval", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" }
            };

            // NOTE: In many apps this provider reads connection strings from config.
            // For this delta test, we only need to assert the *new* behavior: Regex timeout is used.
            // Initialize will throw if the connection string is not present; we skip it.

            // Act + Assert
            // The vulnerability fix added a timeout overload to Regex.IsMatch.
            // We can invoke the regex call via reflection to avoid needing a real DB.
            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "ChangePassword",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            // If method missing, fail clearly.
            Assert.NotNull(method);

            // Build a password that triggers catastrophic backtracking for ^(a+)+$
            string evil = new string('a', 30000) + "!";

            // We expect an ArgumentException from ChangePassword when regex check fails,
            // or a TargetInvocationException wrapping RegexMatchTimeoutException.
            var ex = Assert.ThrowsAny<Exception>(() =>
                method!.Invoke(provider, new object[] { "user", "old", evil }));

            // Assert: ensure the timeout exception exists somewhere in the chain.
            Assert.Contains("RegexMatchTimeoutException", ex.ToString());
        }
    }
}
