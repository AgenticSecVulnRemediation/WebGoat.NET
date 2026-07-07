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
                DateTime.UtcNow.AddMinutes(30),
                false,
                "userdata");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignoredCookieId", "ignoredCookieValue");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.False(string.IsNullOrWhiteSpace(cookie.Value)); // encrypted ticket stored
        }
    }
}
