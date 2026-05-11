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
        public void SetCookie_WhenCalled_SetsHttpOnlyTrueOnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(10),
                false,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_WhenTicketIsPersistent_PreservesExpiration()
        {
            // Arrange
            var expiration = DateTime.Now.AddDays(1);
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                expiration,
                true,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.Equal(expiration, cookie.Expires);
            Assert.True(cookie.HttpOnly);
        }
    }
}
