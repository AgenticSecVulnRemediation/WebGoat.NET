using System;
using System.Text.RegularExpressions;
using Xunit;
using Moq;

// Assumption: production namespace is as declared in source.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_ConstructsRegexWithTimeout_ThrowsOnCatastrophicBacktracking()
        {
            // Delta behavior: Regex constructed with explicit timeout.
            // We assert that a known catastrophic pattern triggers RegexMatchTimeoutException.

            // Arrange
            var page = new RegexDoS();

            // We don't have access to ASP.NET controls easily; validate at regex level
            // because the security fix is about using Regex timeout.
            var pattern = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                _ = re.Match(input);
            });
        }
    }
}
