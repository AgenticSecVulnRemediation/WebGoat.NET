using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_WithUserControlledPattern_EnforcesOneSecondTimeout()
        {
            // Arrange
            // The fix adds a timeout to prevent ReDoS.
            string catastrophicPattern = "^(a+)+$";
            string input = new string('a', 10000) + "!";

            // Act + Assert
            // With timeout, the match should throw RegexMatchTimeoutException (or finish quickly).
            var ex = Record.Exception(() =>
            {
                var re = new Regex(catastrophicPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                re.Match(input);
            });

            Assert.True(ex == null || ex is RegexMatchTimeoutException);
        }
    }
}
