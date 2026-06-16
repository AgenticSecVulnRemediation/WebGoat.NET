using System;
using System.Web;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerCookieFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecure_OnAuthCookie()
        {
            // Arrange
            // Delta for PR #3251: sets HttpOnly and Secure.
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddMinutes(5),
                false,
                "user",
                FormsAuthentication.FormsCookiePath);

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
