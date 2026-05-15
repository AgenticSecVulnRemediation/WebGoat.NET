using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexCtor_WithTimeout_ThrowsRegexMatchTimeoutException_OnCatastrophicBacktracking()
        {
            // Arrange
            var userName = "(a+)+$";
            var password = new string('a', 20000);
            var re = new Regex(userName, RegexOptions.None, TimeSpan.FromSeconds(1));

            // Act + Assert
            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(password));
        }
    }
}
