using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: source file is under WebGoat/Code and project uses xUnit for tests.
// This delta test targets the security fix adding Regex timeouts to mitigate ReDoS.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_PasswordStrengthRegexThatBacktracks_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Set PasswordStrengthRegularExpression to a pattern known to be vulnerable to catastrophic backtracking.
            SetStaticField("_passwordStrengthRegularExpression", "^(a+)+$");

            // Ensure it is non-empty so the code path is executed.
            Assert.False(string.IsNullOrEmpty(provider.PasswordStrengthRegularExpression));

            // Use a string that tends to trigger catastrophic backtracking.
            string attackPassword = new string('a', 20000) + "!";

            // Act + Assert
            // With the fix, Regex.IsMatch is invoked with a timeout (500ms) which should raise RegexMatchTimeoutException.
            Assert.Throws<RegexMatchTimeoutException>(() => provider.ChangePassword("user", "old", attackPassword));
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var type = typeof(SQLiteMembershipProvider);
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException($"Expected private static field '{fieldName}' to exist.");
            }
            field.SetValue(null, value);
        }
    }
}
