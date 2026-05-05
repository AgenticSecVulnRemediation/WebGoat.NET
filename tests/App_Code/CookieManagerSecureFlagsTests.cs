using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecure()
        {
            // Delta test: HttpOnly and Secure flags were added to auth cookie.

            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(10),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.False(string.IsNullOrWhiteSpace(cookie.Value));
        }
    }
}
