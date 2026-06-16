using System;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerHttpOnlyOnlyTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnly_OnAuthCookie()
        {
            // Arrange
            // Delta for PR #3256: sets HttpOnly.
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
        }
    }
}
