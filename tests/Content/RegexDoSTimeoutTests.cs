using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTimeoutTests
    {
        [Fact]
        public void RegexCreatedWithTimeout_PathologicalInputThrowsRegexMatchTimeoutException()
        {
            // Arrange: pattern that causes catastrophic backtracking
            var pattern = "^(a+)+$";
            var input = new string('a', 10000) + "!";

            // Act + Assert
            var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1));
            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(input));
        }
    }
}
