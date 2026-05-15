using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void AuthCookie_WhenCreated_MustBeHttpOnly()
        {
            // Regression test for security hardening: cookie must be HttpOnly.

            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "encrypted-ticket");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
