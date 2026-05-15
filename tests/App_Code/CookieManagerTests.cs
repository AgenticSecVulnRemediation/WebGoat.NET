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
        public void SetCookie_SetsSecureAndHttpOnlyFlags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.Secure);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_PersistentTicket_SetsExpiresToTicketExpiration()
        {
            // Arrange
            var expiration = DateTime.UtcNow.AddDays(1);
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                expiration,
                isPersistent: true,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.Equal(expiration, cookie.Expires);
        }
    }
}
