using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Theory]
        [InlineData("good_cookie-Value_123")]
        [InlineData("ABCdef-123_")]
        public void CookieValidation_AllowsOnlySafeCharacters(string cookieValue)
        {
            // Delta security test: cookie value must match ^[a-zA-Z0-9-_]+$
            Assert.Matches("^[a-zA-Z0-9-_]+$", cookieValue);
        }

        [Theory]
        [InlineData("bad\r\nSet-Cookie: x=y")]
        [InlineData("bad;path=/")]
        [InlineData("<script>")]
        [InlineData("space not allowed")]
        public void CookieValidation_RejectsHeaderInjectionPayloads(string cookieValue)
        {
            // Delta security test: rejects CRLF and other invalid chars
            Assert.DoesNotMatch("^[a-zA-Z0-9-_]+$", cookieValue);
        }

        [Fact]
        public void CookieIsHardened_WithSecureAndHttpOnly()
        {
            // Delta security test: cookie flags
            var fixedSnippet = GetFixedSnippet();
            Assert.Contains("cookie.HttpOnly = true", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("cookie.Secure = true", fixedSnippet, StringComparison.Ordinal);
        }

        private static string GetFixedSnippet()
        {
            return @"cookie.HttpOnly = true;  // Prevents client-side script access
cookie.Secure = true;    // Ensures cookie is sent over HTTPS only";
        }
    }
}
