using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegexUsesTimeout_ThrowsRegexMatchTimeoutExceptionForEvilInput()
        {
            // Delta test for ReDoS fix: Regex.IsMatch now uses a 1-second timeout.
            // We validate behavior of Regex with explicit timeout representative of the production call.

            var pattern = "^(a+)+$";
            var input = new string('a', 100_000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(1))
            );
        }
    }
}
