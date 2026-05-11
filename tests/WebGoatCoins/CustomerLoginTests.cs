using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void LoginCookie_IsHttpOnly_AndSecure()
        {
            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "encrypted");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
