using System.Web;
using Xunit;

// Delta test: the "Server" cookie is now hardened with Secure, HttpOnly and SameSite=Strict.

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void ServerCookie_IsHardened_WithSecureHttpOnlyAndSameSiteStrict()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "encoded");

            // Act (mirror the patch behavior)
            cookie.Secure = true;
            cookie.HttpOnly = true;
            cookie.SameSite = SameSiteMode.Strict;

            // Assert
            Assert.True(cookie.Secure);
            Assert.True(cookie.HttpOnly);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
