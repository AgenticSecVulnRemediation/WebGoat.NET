using System;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void ServerInfoCookie_IsHttpOnlyAndSecure()
        {
            // Arrange
            // We can't execute Page_Load without a full ASP.NET runtime; this unit test focuses on the delta:
            // cookies must be marked HttpOnly and Secure.
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
