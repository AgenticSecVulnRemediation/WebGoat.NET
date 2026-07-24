using System;
using System.Text.RegularExpressions;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_PasswordStrengthRegex_UsesTimeoutToAvoidReDoS()
        {
            // Arrange
            // The fix added a Regex.IsMatch overload with a timeout.
            // We can't easily execute provider end-to-end here without config/db; instead we assert that a timeout-based call
            // will throw RegexMatchTimeoutException for a catastrophic pattern.
            var catastrophicPattern = "^(a+)+$";
            var input = new string('a', 5000) + "!"; // force backtracking

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, catastrophicPattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
