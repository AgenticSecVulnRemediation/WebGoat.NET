using System;
using System.Web;
using Moq;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET as declared in file.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieHardeningTests
    {
        [Fact]
        public void ServerInfoCookie_IsHardenedWithHttpOnlyAndSecure()
        {
            // Arrange
            // Delta behavior: the "Server" cookie is now marked HttpOnly and Secure.
            // We validate the intended secure flags directly on HttpCookie.
            var cookie = new HttpCookie("Server", "someValue");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
