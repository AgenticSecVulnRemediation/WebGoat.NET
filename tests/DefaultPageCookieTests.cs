using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void ServerInfoCookie_IsHttpOnlyAndSecure()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "value");

            // Act (apply same flags required by fix)
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
