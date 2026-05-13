using System;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesAuthCookie_SetsHttpOnly()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "userData");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.True(cookie.HttpOnly);
        }
    }
}
