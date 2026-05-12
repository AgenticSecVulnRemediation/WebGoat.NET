using System;
using System.Web;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureFlags_OnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), false, "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
