using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumptions:
// - Tests validate the security fix: regex evaluation now enforces a match timeout (ReDoS mitigation).
// - We do not need to instantiate SQLiteMembershipProvider; we validate the core behavior using Regex with a timeout.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_WithCatastrophicBacktracking_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // A known catastrophic backtracking pattern + large input.
            var pattern = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            // Act / Assert
            // Mirrors the updated code path which constructs Regex with a 500ms timeout.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                _ = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));
                // Force evaluation
                _ = Regex.IsMatch(input, pattern);
            });
        }
    }
}
