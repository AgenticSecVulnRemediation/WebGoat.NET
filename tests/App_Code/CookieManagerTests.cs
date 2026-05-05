using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_Always_SetsHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_PersistentTicket_SetsCookieExpiresToTicketExpiration()
        {
            // Arrange
            var expiration = DateTime.UtcNow.AddDays(1);
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                expiration,
                true,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.Equal(expiration, cookie.Expires);
        }
    }
}
