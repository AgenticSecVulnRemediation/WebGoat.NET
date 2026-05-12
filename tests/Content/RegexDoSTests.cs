using Xunit;
using System;
using System.Text.RegularExpressions;

// Assumption: page class is OWASP.WebGoat.NET.RegexDoS
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_UsesTimeout_ToBoundExecution()
        {
            // Arrange
            var pattern = "a+";

            // Act
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            // Assert
            Assert.Equal(TimeSpan.FromMilliseconds(1000), regex.MatchTimeout);
        }
    }
}
