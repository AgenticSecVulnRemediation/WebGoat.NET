using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderChangePasswordRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithPathologicalRegex_DoesNotHangAndThrowsArgumentException()
        {
            // Arrange
            // Force a catastrophic backtracking pattern that would hang without a timeout.
            var provider = new SQLiteMembershipProvider();

            // Set the private static password strength regex used by ChangePassword.
            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            // Seed other static fields minimally so we fail before DB usage.
            // Ensure min length/non-alphanumeric requirements are satisfied so we hit the regex check.
            typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 1);
            typeof(SQLiteMembershipProvider)
                .GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, 0);

            // Act + Assert
            // The new password is crafted to trigger backtracking against ^(a+)+$.
            // With the fix, Regex.IsMatch is invoked with a timeout; it should return false or throw RegexMatchTimeoutException.
            // Either way, ChangePassword must not hang indefinitely.
            var ex = Record.Exception(() =>
            {
                try
                {
                    // We don't care about old password validation; we just want to ensure the regex evaluation is bounded.
                    // This call will likely fail earlier due to missing connection string when it reaches DB, but only after
                    // the regex check. So we call with a newPassword that should fail the regex check first.
                    provider.ChangePassword("user", "old", new string('a', 50000) + "!X");
                }
                catch (RegexMatchTimeoutException)
                {
                    // acceptable: indicates timeout enforcement
                    throw;
                }
            });

            Assert.True(ex is ArgumentException or RegexMatchTimeoutException,
                $"Expected ArgumentException (regex mismatch) or RegexMatchTimeoutException, got {ex?.GetType().FullName}");
        }
    }
}
