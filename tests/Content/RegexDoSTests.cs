using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_UsesTimeout_ThrowsRegexMatchTimeoutException_ForCatastrophicBacktracking()
        {
            // Arrange
            // Delta behavior: Regex constructed with TimeSpan.FromSeconds(1).
            var pattern = "^(a+)+$";
            var attack = new string('a', 20000) + "!";

            // Act / Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1));
                re.Match(attack);
            });
        }
    }
}
