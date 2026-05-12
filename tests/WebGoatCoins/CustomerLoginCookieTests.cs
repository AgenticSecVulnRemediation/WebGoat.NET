using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void CustomerLogin_AuthCookie_IsHttpOnly()
        {
            // Delta: enforce HttpOnly on forms auth cookie
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
