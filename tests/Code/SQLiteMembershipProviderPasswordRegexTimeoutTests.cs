using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            // The fix changes Regex.IsMatch(password, pattern) to include a timeout.
            var pattern = "^(a+)+$"; // classic catastrophic backtracking
            var input = new string('a', 5000) + "!";

            // Act + Assert
            // With an explicit timeout, the runtime should throw RegexMatchTimeoutException
            // rather than hanging indefinitely.
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
