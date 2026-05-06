using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureCookieTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecure()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(1), false, "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
