using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTimeoutTests
    {
        [Fact]
        public void Regex_WithTimeout_ThrowsRegexMatchTimeoutException_ForEvilPattern()
        {
            // Delta behavior: Regex constructed with a timeout.
            // Demonstrate that catastrophic backtracking is interrupted by timeout.
            string pattern = "^(a+)+$";
            string input = new string('a', 50000) + "!";

            var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1));

            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(input));
        }
    }
}
