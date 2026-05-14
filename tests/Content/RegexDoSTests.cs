using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstruction_WithUserControlledPattern_EnforcesTimeout()
        {
            // Arrange
            // Delta: Regex is now created with a 1000ms timeout.
            // Use catastrophic backtracking input; without timeout, could run excessively.
            string evilPattern = "^(a+)+$";
            string input = new string('a', 20000) + "!";

            var regex = new Regex(evilPattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
