using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureFlagTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsSecureTrueOnAuthCookie()
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
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignoredId", "ignoredValue");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
