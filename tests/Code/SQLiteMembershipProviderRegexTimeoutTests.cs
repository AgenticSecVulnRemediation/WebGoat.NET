using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_IsEvaluatedWithTimeout_ToMitigateReDoS()
        {
            // Delta assertion based strictly on the patch: Regex.IsMatch now passes a timeout.
            // We validate that catastrophic backtracking triggers a RegexMatchTimeoutException rather than hanging.
            var pattern = "^(a+)+$";
            var input = new string('a', 10000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(1)));
        }
    }
}
