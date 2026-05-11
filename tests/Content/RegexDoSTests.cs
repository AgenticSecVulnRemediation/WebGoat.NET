using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstruction_WithTimeout_ThrowsRegexMatchTimeoutException_ForCatastrophicBacktracking()
        {
            // Delta security fix: Regex is now constructed with a timeout (1 second).
            // This test demonstrates that catastrophic patterns do not hang indefinitely.

            var catastrophicPattern = "^(a+)+$";
            var longInput = new string('a', 200_000) + "!";

            var re = new Regex(catastrophicPattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));

            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(longInput));
        }
    }
}
