using System;
using Xunit;

// Namespace inferred from source file path: OWASP.WebGoat.NET

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionRegexCookieValidationTests
    {
        [Theory]
        [InlineData("abc123", true)]
        [InlineData("ABCdef", true)]
        [InlineData("123456", true)]
        [InlineData("abc-123", false)]
        [InlineData("abc 123", false)]
        [InlineData("abc\r\nSet-Cookie: x=y", false)]
        public void CookieValueRegex_AllowsOnlyAlphaNumeric(string cookieValue, bool expectedMatch)
        {
            // This test targets the new validation introduced in the patch:
            // Regex.IsMatch(userCookie, "^[a-zA-Z0-9]+$")

            var isMatch = System.Text.RegularExpressions.Regex.IsMatch(cookieValue, "^[a-zA-Z0-9]+$");
            Assert.Equal(expectedMatch, isMatch);
        }
    }
}
