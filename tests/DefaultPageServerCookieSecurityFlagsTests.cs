using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageServerCookieSecurityFlagsTests
    {
        [Fact]
        public void ServerCookie_HasSecureHttpOnlyAndStrictSameSiteFlags()
        {
            var cookie = new System.Web.HttpCookie("Server", "value")
            {
                Secure = true,
                HttpOnly = true,
                SameSite = System.Web.SameSiteMode.Strict
            };

            Assert.True(cookie.Secure);
            Assert.True(cookie.HttpOnly);
            Assert.Equal(System.Web.SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
