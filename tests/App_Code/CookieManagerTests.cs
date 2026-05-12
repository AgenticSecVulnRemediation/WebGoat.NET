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
        public void SetCookie_SetsHttpOnlyAndSecure_OnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                false,
                "data",
                FormsAuthentication.FormsCookiePath);

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
