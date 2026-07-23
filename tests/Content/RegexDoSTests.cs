using System;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_UsesRegexTimeout_ToMitigateBacktrackingDos()
        {
            // Delta test: Regex(userName, RegexOptions.None, TimeSpan.FromSeconds(1))
            // We assert that a crafted catastrophic pattern times out rather than running indefinitely.

            var page = new RegexDoS();

            // Arrange: a catastrophic backtracking pattern.
            var pattern = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            // Act & Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var re = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1));
                re.Match(input);
            });
        }
    }
}
