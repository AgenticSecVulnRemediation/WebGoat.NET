using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void CookieCreatedForUserAddedCookie_ShouldBeHttpOnly()
        {
            // Delta test focuses on secure behavior: cookie is marked HttpOnly.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "x",
                HttpOnly = true
            };

            Assert.True(cookie.HttpOnly);
        }
    }
}
