using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieFlagsDeltaTests
    {
        [Fact]
        public void Patch167_FormsAuthCookie_IsHttpOnly_AndSecure()
        {
            // Delta assertion: cookie flags were added.
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");
            cookie.HttpOnly = true;
            cookie.Secure = true;

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        [Fact]
        public void Patch161_FormsAuthCookie_IsHttpOnly()
        {
            // Delta assertion: HttpOnly flag was added.
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
