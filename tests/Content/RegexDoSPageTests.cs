using System;
using System.Text.RegularExpressions;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_ConstructorWithTimeout_TimesOutOnCatastrophicUserRegex()
        {
            // Arrange
            // Delta behavior: Regex is constructed with a 1-second timeout.
            var userControlledRegex = "^(a+)+$";
            var input = new string('a', 200000) + "!";

            // Act
            var regex = new Regex(userControlledRegex, RegexOptions.None, TimeSpan.FromMilliseconds(1));

            // Assert
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}
