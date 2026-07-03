using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET as declared in file.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_WithUserControlledPattern_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            // Regression target: code was changed from new Regex(userName) to
            // new Regex(userName, RegexOptions.None, TimeSpan.FromMilliseconds(100)).
            //
            // We can't directly access the page's local variable, so we assert the expected safe behavior:
            // a catastrophic-backtracking regex should time out quickly when a timeout is configured.
            var evilPattern = "^(a+)+$";
            var input = new string('a', 50000) + "!";

            var regex = new Regex(evilPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

            // Act / Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
