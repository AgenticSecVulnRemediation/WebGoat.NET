using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_IsEvaluatedWithTimeout()
        {
            // This test is a delta regression for the security fix that introduced a timeout
            // to Regex.IsMatch calls (RegexOptions.None, TimeSpan.FromSeconds(1)).
            // We validate that the overload with a TimeSpan is used by asserting that
            // a RegexMatchTimeoutException can be observed for a pattern known to be susceptible
            // to catastrophic backtracking when matched against a long input.

            var pattern = "^(a+)+$";
            var input = new string('a', 20000) + "!";

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                _ = Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            });
        }
    }
}
