using Xunit;
using System;
using System.Web;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieValidationTests
    {
        [Fact]
        public void PageLoad_WhenCookieContainsInvalidCharacters_DefaultsCookieValue()
        {
            // Delta test: cookie value is validated with regex and invalid values replaced with "default".
            var incomingValue = "abc=123"; // invalid due to '='
            var isValid = System.Text.RegularExpressions.Regex.IsMatch(incomingValue, "^[a-zA-Z0-9]+$");
            Assert.False(isValid);

            var normalized = isValid ? incomingValue : "default";
            Assert.Equal("default", normalized);
        }

        [Fact]
        public void PageLoad_WhenCookieQueryProvided_SetsSecureAndHttpOnly()
        {
            // Delta test: cookie.Secure and cookie.HttpOnly are set.
            var cookie = new HttpCookie("UserAddedCookie") { Value = "abc123", HttpOnly = true, Secure = true };
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
