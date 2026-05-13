using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Namespace inferred from source file path: OWASP.WebGoat.NET.App_Code
namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsHttpOnlyAndSecureFlags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.AddMinutes(30),
                isPersistent: false,
                userData: "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
