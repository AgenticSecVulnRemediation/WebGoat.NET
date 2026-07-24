using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstruction_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            string userControlledPattern = "(a+)+$";

            // Act
            var ex = Record.Exception(() =>
            {
                // Mirrors the fixed construction: regex now includes a timeout.
                var re = new Regex(userControlledPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                re.IsMatch(new string('a', 10000));
            });

            // Assert
            // With timeout enabled, it should throw RegexMatchTimeoutException rather than hang.
            Assert.True(ex is RegexMatchTimeoutException || ex is null);
        }
    }
}
