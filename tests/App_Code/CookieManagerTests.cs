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
        public void SetCookie_Always_SetsHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(10),
                false,
                "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignoredId", "ignoredValue");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_PersistentTicket_SetsCookieExpiresToTicketExpiration()
        {
            // Arrange
            var expiration = DateTime.Now.AddHours(1);
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                expiration,
                true,
                "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignoredId", "ignoredValue");

            // Assert
            Assert.Equal(expiration, cookie.Expires);
        }
    }
}
