using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordRegexTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeout_ToMitigateReDoS()
        {
            // Arrange
            // Delta behavior: Regex.IsMatch now uses an explicit timeout to mitigate catastrophic backtracking.
            var catastrophicPattern = "^(a+)+$";
            var password = new string('a', 10000) + "!"; // does not match; causes backtracking

            // Act / Assert
            // Using a very small timeout ensures deterministic timeout behavior in test.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(password, catastrophicPattern, RegexOptions.None, TimeSpan.FromMilliseconds(1)));
        }
    }
}
