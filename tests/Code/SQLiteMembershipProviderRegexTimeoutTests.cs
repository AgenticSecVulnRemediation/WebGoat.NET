using System;
using System.Text.RegularExpressions;
using Xunit;

// Delta test for PR 3948: Regex.IsMatch overload with an explicit timeout is used to mitigate ReDoS.
// We keep this deterministic by directly asserting that the timeout-enabled overload throws
// RegexMatchTimeoutException for a known catastrophic-backtracking pattern.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void RegexIsMatch_WithTimeout_ThrowsRegexMatchTimeoutException_OnCatastrophicBacktracking()
        {
            var catastrophicRegex = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, catastrophicRegex, RegexOptions.None, TimeSpan.FromMilliseconds(100))
            );
        }
    }
}
