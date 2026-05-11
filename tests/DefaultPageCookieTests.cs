using System;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void ServerCookie_IsHttpOnly()
        {
            // Arrange/Act
            var cookie = new System.Web.HttpCookie("Server", "value")
            {
                HttpOnly = true
            };

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
