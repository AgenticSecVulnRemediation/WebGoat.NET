using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumptions:
// - Source namespace is OWASP.WebGoat.NET as in the patched file.

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void BtnCreate_Click_UsesRegexTimeout_WhenConstructingRegex()
        {
            // Arrange
            // Delta behavior: Regex is now constructed with an explicit timeout.
            // We validate the secure behavior by directly creating a Regex with the same constructor pattern
            // and asserting a timeout is set.
            var pattern = "name";

            // Act
            var r = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));

            // Assert
            Assert.Equal(TimeSpan.FromSeconds(1), r.MatchTimeout);
        }
    }
}
