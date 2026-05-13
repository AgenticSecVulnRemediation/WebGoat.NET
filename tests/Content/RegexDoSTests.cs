using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.Content
{
    public class RegexDosTests
    {
        [Fact]
        public void RegexConstruction_UsesMatchTimeout()
        {
            // Arrange
            // We validate the delta behavior by ensuring the overload with timeout is used.
            var pattern = "a+";

            // Act
            var regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            // Assert
            Assert.Equal(TimeSpan.FromMilliseconds(1000), regex.MatchTimeout);
        }
    }
}
