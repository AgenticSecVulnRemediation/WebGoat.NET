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
        public void SetCookie_WithFormsAuthTicket_SetsHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.False(string.IsNullOrEmpty(cookie.Value));
        }
    }
}
