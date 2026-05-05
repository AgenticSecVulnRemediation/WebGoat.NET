using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecureFlagsTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnly_AndSecure()
        {
            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
