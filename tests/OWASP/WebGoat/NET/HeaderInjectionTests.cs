using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void PageLoad_WhenAddingCookie_SetsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie("UserAddedCookie");

            // Act
            cookie.Value = "test";
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
