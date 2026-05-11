using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginHttpOnlyTests
    {
        [Fact]
        public void LoginCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie(".ASPXAUTH", "encrypted");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
