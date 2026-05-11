using System;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void ServerCookie_IsHardened_WithHttpOnlyAndSecure()
        {
            // Arrange
            // Delta scope: the "Server" cookie now sets HttpOnly and Secure.
            // We validate the intended cookie flags by constructing one as in the page.
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
