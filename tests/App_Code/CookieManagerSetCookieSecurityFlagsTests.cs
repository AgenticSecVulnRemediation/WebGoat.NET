using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSetCookieSecurityFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureToTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.AddMinutes(30),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, cookieId: "id", cookieValue: "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        [Fact]
        public void SetCookie_WhenTicketIsPersistent_SetsCookieExpiresToTicketExpiration()
        {
            // Arrange
            DateTime expiration = DateTime.UtcNow.AddDays(1);
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: expiration,
                isPersistent: true,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, cookieId: "id", cookieValue: "value");

            // Assert
            Assert.Equal(expiration, cookie.Expires);
        }
    }
}
