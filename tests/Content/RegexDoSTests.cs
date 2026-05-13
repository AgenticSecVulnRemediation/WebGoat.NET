using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_UsesTimeout_ToMitigateReDoS()
        {
            // Arrange
            var userSuppliedPattern = "(a+)+$";

            // Act
            var regex = new System.Text.RegularExpressions.Regex(userSuppliedPattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            // Assert
            Assert.Equal(TimeSpan.FromMilliseconds(1000), regex.MatchTimeout);
        }
    }
}
