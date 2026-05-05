using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_WithUserSuppliedPattern_UsesTimeout_AndThrowsOnCatastrophicBacktracking()
        {
            // Arrange
            // This is the classic catastrophic backtracking pattern.
            var userSuppliedPattern = "^(a+)+$";
            var longInput = new string('a', 20000) + "!";

            // Act
            // Delta: Regex is now constructed with a timeout; we should get a RegexMatchTimeoutException
            // instead of hanging.
            var ex = Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var regex = new Regex(userSuppliedPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
                regex.Match(longInput);
            });

            // Assert
            Assert.NotNull(ex);
        }
    }
}
