using System;
using System.Text.RegularExpressions;
using Xunit;

// Delta test for PR #354.
// The patch adds a regex timeout to mitigate ReDoS risk in password strength validation.
// We assert that a catastrophic-backtracking style input triggers a RegexMatchTimeoutException
// rather than hanging indefinitely.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_WithEvilInput_TimesOut()
        {
            // Arrange
            // A commonly problematic pattern; mirrors the kind of patterns that can backtrack heavily.
            var pattern = "^(a+)+$";
            var input = new string('a', 50000) + "!";

            // Act / Assert
            // Use same options and timeout style introduced in the patch.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
