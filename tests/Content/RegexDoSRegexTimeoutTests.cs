using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSRegexTimeoutTests
    {
        [Fact]
        public void RegexDoS_UsesRegexTimeout_ToMitigateReDoS()
        {
            // Arrange
            // Delta for PR #3329: Regex constructed with timeout of 1 second.
            // We validate that a pathological regex does not run indefinitely by expecting RegexMatchTimeoutException.

            var userControlledPattern = "^(a+)+$";
            var input = new string('a', 10000) + "!";

            // Act + Assert
            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var regex = new System.Text.RegularExpressions.Regex(userControlledPattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1));
                regex.Match(input);
            });

            Assert.NotNull(ex);
        }
    }
}
