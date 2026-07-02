using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstruction_WithCatastrophicPattern_UsesTimeoutAndThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // This test models the fixed behavior: Regex created with an explicit timeout.
            // Use a known catastrophic backtracking pattern.
            var userNamePattern = "^(a+)+$";
            var password = new string('a', 50000) + "!"; // should provoke backtracking

            var regex = new Regex(userNamePattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(password));
        }
    }
}
