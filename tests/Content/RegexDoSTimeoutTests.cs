using Xunit;
using System.Text.RegularExpressions;
using System;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTimeoutTests
    {
        [Fact]
        public void RegexMatch_WhenCatastrophicBacktracking_ThrowsRegexMatchTimeoutException()
        {
            // Regression test: regex matching must be protected by a timeout to prevent ReDoS.
            // Arrange
            string evilPattern = "^(a+)+$";
            string input = new string('a', 50000) + "!";

            var re = new Regex(evilPattern, RegexOptions.None, TimeSpan.FromMilliseconds(1));

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(input));
        }
    }
}
