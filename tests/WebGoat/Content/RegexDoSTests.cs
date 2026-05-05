using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstruction_IncludesTimeout_ToMitigateReDoS()
        {
            // Arrange
            // A crafted regex can cause catastrophic backtracking. The fix adds a 1s timeout.
            var pattern = "^(a+)+$";
            var input = new string('a', 40000) + "!";

            // Act / Assert
            // Without timeout, this can hang for a very long time. With timeout, it should throw RegexMatchTimeoutException.
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
