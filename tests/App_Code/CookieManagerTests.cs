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
        public void SetCookie_CreatesHttpOnlyAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), false, "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.True(cookie.HttpOnly);
        }
    }
}
