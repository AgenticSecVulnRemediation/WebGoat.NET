using System;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureFlags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.Now, DateTime.Now.AddMinutes(1), true, "role");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
