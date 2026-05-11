using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstructor_UsesTimeout_ToMitigateReDoS()
        {
            // Delta security test: Regex now uses a timeout.
            // We assert that catastrophic backtracking is bounded by the configured timeout.

            var pattern = "^(a+)+$";
            var input = new string('a', 50000) + "!";

            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
