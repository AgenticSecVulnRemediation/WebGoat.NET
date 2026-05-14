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
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "",
                FormsAuthentication.FormsCookiePath);

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void SetCookie_Always_SetsSecureTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "",
                FormsAuthentication.FormsCookiePath);

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.Secure);
        }
    }
}
