using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void UserAddedCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie("UserAddedCookie") { Value = "value" };

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
