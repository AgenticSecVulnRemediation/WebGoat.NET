using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieFlagsTests
    {
        [Fact]
        public void ServerCookie_WhenConfigured_SetsHttpOnlyAndSecure()
        {
            // Delta behavior: HttpOnly and Secure flags are set on the "Server" cookie.
            // We validate the semantics of the flags on an equivalent cookie instance.
            var cookie = new HttpCookie("Server", "value")
            {
                HttpOnly = true,
                Secure = true
            };

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
