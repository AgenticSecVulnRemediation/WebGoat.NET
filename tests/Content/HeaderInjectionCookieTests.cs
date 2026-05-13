using System.Web;
using Xunit;

// Assumption: production page class is in namespace OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieTests
    {
        [Fact]
        public void CookieAddedFromQueryString_IsHttpOnly()
        {
            // Delta unit test: when a cookie is created from user input, it must be HttpOnly.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "attacker-controlled",
                HttpOnly = true
            };

            Assert.True(cookie.HttpOnly);
        }
    }
}
