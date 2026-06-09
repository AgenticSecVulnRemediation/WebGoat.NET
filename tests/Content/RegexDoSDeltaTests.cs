using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSDeltaTests
    {
        [Fact]
        public void RegexConstructor_WithTimeout_ThrowsRegexMatchTimeoutExceptionOnCatastrophicBacktracking()
        {
            // Arrange
            // Classic catastrophic-backtracking pattern. With a timeout, Match should throw RegexMatchTimeoutException.
            var pattern = "^(a+)+$";
            var input = new string('a', 20000) + "!";

            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
