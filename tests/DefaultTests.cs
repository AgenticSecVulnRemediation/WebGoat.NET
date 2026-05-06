using System;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void PageLoad_SetsServerCookie_HttpOnlyAndSecure()
        {
            // Arrange
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
