using System;
using System.Linq;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicRegex_ThrowsRegexMatchTimeoutException()
        {
            // This delta adds a regex timeout to the password strength check.
            // We verify it by forcing a catastrophic pattern and a long input that would otherwise hang.

            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Force a dangerous regex into the private static field.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_passwordStrengthRegularExpression", "^(a+)+$");

            // Also set min lengths low enough to reach the regex check.
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredPasswordLength", 1);
            SetPrivateStaticField(typeof(SQLiteMembershipProvider), "_minRequiredNonAlphanumericCharacters", 0);

            var newPassword = new string('a', 20000) + "!";

            // Act / Assert
            // We don't care about auth/DB; CheckPassword will likely throw later.
            // The important thing is that the regex check itself enforces a timeout.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                try
                {
                    provider.ChangePassword("user", "old", newPassword);
                }
                catch (ArgumentException)
                {
                    // If the provider throws due to regex mismatch without timing out, that's acceptable
                    // only if it didn't hang. But the timeout should trigger for this payload.
                    throw;
                }
            });
        }

        private static void SetPrivateStaticField(Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
