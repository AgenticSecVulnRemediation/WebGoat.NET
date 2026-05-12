using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code (based on file path and source file namespace).
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesHttpOnlyCookie_ReturnsCookieWithHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(10),
                false,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_WithPersistentTicket_SetsCookieExpiresToTicketExpiration()
        {
            // Arrange
            var expiration = DateTime.UtcNow.AddHours(1);
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                expiration,
                true,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.Equal(expiration, cookie.Expires);
            Assert.True(cookie.HttpOnly);
        }
    }
}
