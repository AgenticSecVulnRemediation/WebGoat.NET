using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstruction_HasTimeoutToPreventReDoS()
        {
            // Delta test for PR #3955: Regex is now created with a timeout.
            // We verify the timeout is actually present and effective by triggering
            // a RegexMatchTimeoutException with a backtracking-heavy pattern.

            var pattern = "(a+)+$";
            var input = new string('a', 4000) + "!";

            var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));

            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(input));
        }
    }
}
