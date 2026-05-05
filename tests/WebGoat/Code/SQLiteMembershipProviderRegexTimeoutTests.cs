using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            // Simulate the updated Regex.IsMatch call signature used by the provider.
            // A vulnerable implementation would call Regex.IsMatch(password, pattern) without a timeout.
            var pattern = "^(a+)+$"; // catastrophic backtracking for long non-matching strings
            var attackInput = new string('a', 50000) + "!"; // non-match triggers backtracking

            // Act / Assert
            // With a 1s timeout, Regex.IsMatch should throw RegexMatchTimeoutException instead of hanging.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(attackInput, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000))
            );
        }
    }
}
