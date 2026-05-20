using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: source classes live under namespace TechInfoSystems.Data.SQLite as declared in the patched file.
// This test uses reflection to set the provider's private static password regex field.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Force a catastrophic-backtracking pattern and bypass the "only if regex is non-empty" guard.
            // Pattern: (a+)+$ is known to backtrack heavily for long strings like aaaaa...!
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");

            // Make sure min-length checks don't trip first.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredPasswordLength", 1);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredNonAlphanumericCharacters", 0);

            // We need CheckPassword(username, oldPassword, ...) to pass to reach Regex.IsMatch(newPassword, ...).
            // We can't easily hit the DB here, so we invoke the regex-check path indirectly by calling the private
            // password validation logic via reflection.
            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "ChangePassword",
                BindingFlags.Instance | BindingFlags.Public);

            // Act + Assert
            // Any call that reaches Regex.IsMatch(newPassword, pattern, ..., TimeSpan.FromSeconds(1)) with long input
            // should throw RegexMatchTimeoutException.
            // Note: the method may throw other exceptions first depending on environment; we only assert that a timeout
            // is thrown when regex evaluation occurs.
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
            {
                // Intentionally long input causing catastrophic backtracking.
                var newPassword = new string('a', 50000) + "!";

                // Username/oldPassword values are arbitrary; if the environment prevents reaching the regex check,
                // this test will fail and must be adapted alongside provider wiring.
                method!.Invoke(provider, new object?[] { "user", "old", newPassword });
            });
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
