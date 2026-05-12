using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesHttpOnlyCookie_ReturnsCookieWithHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.AddMinutes(5),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
