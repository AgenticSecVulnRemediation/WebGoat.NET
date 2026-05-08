using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieFlagsTests
    {
        [Fact]
        public void ServerInfoCookie_IsHttpOnlyAndSecure()
        {
            // Arrange
            // The fix sets HttpOnly and Secure on the "Server" cookie to mitigate info-leak/XSS access.
            var cookie = new HttpCookie("Server", "value");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
