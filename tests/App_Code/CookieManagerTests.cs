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
            var cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
