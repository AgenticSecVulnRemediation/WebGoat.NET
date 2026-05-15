using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesCookieWithHttpOnly_True()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(10), false, "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
