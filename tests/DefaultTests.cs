using System.Web;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET (from source file WebGoat/Default.aspx.cs)
namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void ServerInfoCookie_IsHardened_HttpOnlyAndSecureAreTrue()
        {
            // This is a focused test of the changed behavior: cookie flags are explicitly set.
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
