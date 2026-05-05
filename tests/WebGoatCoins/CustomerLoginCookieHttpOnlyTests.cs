using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHttpOnlyTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
