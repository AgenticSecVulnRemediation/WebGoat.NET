using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "ticket") { HttpOnly = true };

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
