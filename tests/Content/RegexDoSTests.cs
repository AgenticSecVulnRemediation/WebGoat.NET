using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexCtor_WithTimeout_DoesNotThrow_ForSimplePattern()
        {
            // Arrange
            // Delta-focused: verifies new Regex overload with timeout is usable.
            var pattern = "abc";

            // Act
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            // Assert
            Assert.NotNull(regex);
        }
    }
}
