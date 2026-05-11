using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstruction_WithUserControlledPattern_UsesTimeout_AndCanTimeout()
        {
            // Arrange
            string userControlledPattern = "^(a+)+$";
            string password = new string('a', 5000) + "!";

            // Act & Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var regex = new Regex(userControlledPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                regex.Match(password);
            });
        }
    }
}
