using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegexDoesNotHang_ThrowsWithinTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // We rely on Initialize validating regex, but we don't want to require a real web.config.
            // Set the private static field _passwordStrengthRegularExpression via reflection to a catastrophic regex.
            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.Static | BindingFlags.NonPublic);

            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            // Also set minimum requirements to allow regex check to be reached
            typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredPasswordLength", BindingFlags.Static | BindingFlags.NonPublic)!
                .SetValue(null, 1);

            typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.Static | BindingFlags.NonPublic)!
                .SetValue(null, 0);

            // Act + Assert
            // With the fix, Regex.IsMatch uses a match timeout and will throw RegexMatchTimeoutException
            // instead of hanging on catastrophic backtracking.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                // Choose a long input that would normally cause catastrophic backtracking.
                var badPassword = new string('a', 5000) + "!";

                // Call CreateUser; it will hit the passwordStrengthRegularExpression check.
                provider.CreateUser(
                    username: "user",
                    password: badPassword,
                    email: "user@example.com",
                    passwordQuestion: null,
                    passwordAnswer: null,
                    isApproved: true,
                    providerUserKey: null,
                    status: out _);
            });
        }
    }
}
