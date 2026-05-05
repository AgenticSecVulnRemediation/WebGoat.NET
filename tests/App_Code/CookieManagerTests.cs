using System;
using System.IO;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnly_OnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.AddMinutes(30),
                isPersistent: false,
                userData: "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.True(cookie.HttpOnly);
        }
    }
}
