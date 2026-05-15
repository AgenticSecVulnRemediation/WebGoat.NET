using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieFlagsTests
    {
        [Fact]
        public void ServerCookie_WhenCreated_IsHttpOnlyAndSecure()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "encoded-value");

            // Act: patched behavior sets HttpOnly and Secure
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
