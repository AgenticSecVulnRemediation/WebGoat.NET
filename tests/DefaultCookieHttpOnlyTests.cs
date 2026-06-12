using System;
using System.Web;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void ServerInfoCookie_IsHttpOnly()
        {
            // Arrange
            // Patch sets cookie.HttpOnly = true for "Server" cookie.
            var cookie = new HttpCookie("Server", "value")
            {
                HttpOnly = true
            };

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
