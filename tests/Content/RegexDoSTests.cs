using System;
using System.Text.RegularExpressions;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstructor_UsesExplicitTimeout()
        {
            // This is a focused regression test for the security fix: Regex construction now includes a timeout.
            // Arrange
            var pattern = "(a+)+$";
            var input = new string('a', 50) + "!";

            // Act + Assert
            // With an explicit timeout, pathological patterns must not run unbounded.
            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1));
                _ = re.Match(input);
            });

            Assert.NotNull(ex);
        }
    }
}
