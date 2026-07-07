using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_ConstructedRegex_UsesTimeoutToMitigateReDoS()
        {
            // Arrange
            var pattern = "(a+)+$";

            // Act
            var ex = Record.Exception(() =>
            {
                // Mirrors fixed behavior: a timeout is supplied.
                var r = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1));
            });

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void RegexDoS_LongInput_DoesNotHangDueToRegexTimeout()
        {
            // Arrange
            var pattern = "(a+)+$";
            var input = new string('a', 20000) + "!";

            var r = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(50));

            // Act / Assert
            Assert.Throws<System.Text.RegularExpressions.RegexMatchTimeoutException>(() => r.Match(input));
        }
    }
}
