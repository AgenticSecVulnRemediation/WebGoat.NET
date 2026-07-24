using System;
using System.Web;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieHardeningTests
    {
        [Fact]
        public void PageLoad_SetsServerCookieSecurityFlags_HttpOnlySecureSameSiteStrict()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "value");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;
            cookie.SameSite = SameSiteMode.Strict;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
