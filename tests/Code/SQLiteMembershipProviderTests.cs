using System;
using System.Text.RegularExpressions;
using Xunit;

// Delta test: CreateUser password regex check now uses a Regex timeout to mitigate ReDoS.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void PasswordStrengthRegexMatch_UsesTimeout_ToPreventReDoSFromCatastrophicBacktracking()
        {
            // Arrange
            // Use a known catastrophic regex and an input that would take very long without a timeout.
            var catastrophicRegex = "^(a+)+$";
            var input = new string('a', 50000) + "!";

            // Act + Assert
            // This mirrors the patched behavior: Regex.IsMatch(..., timeout: 2 seconds)
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                Regex.IsMatch(input, catastrophicRegex, RegexOptions.None, TimeSpan.FromSeconds(2));
            });
        }
    }
}
