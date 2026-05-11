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
        public void SetCookie_WithAnyTicket_SetsHttpOnlyOnAuthCookie()
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
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.True(cookie.HttpOnly);
            Assert.False(string.IsNullOrEmpty(cookie.Value));
        }
    }
}
