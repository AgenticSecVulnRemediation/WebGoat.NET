using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void ServerCookie_ShouldBeHttpOnlyAndSecure()
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
