using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexCreation_WithUserProvidedPattern_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            // This pattern is known to cause catastrophic backtracking on non-matching input.
            string userProvidedPattern = "^(a+)+$";
            string password = new string('a', 5000) + "!";

            // Act
            var regex = new Regex(userProvidedPattern, RegexOptions.None, TimeSpan.FromSeconds(1));

            // Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(password));
        }
    }
}
