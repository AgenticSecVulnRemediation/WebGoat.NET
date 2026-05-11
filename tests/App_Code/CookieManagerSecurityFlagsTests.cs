using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecurityFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureFlags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                "userdata");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
