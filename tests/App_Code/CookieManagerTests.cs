using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code (from CookieManager.cs)
namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnly_OnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now.AddMinutes(-1),
                DateTime.Now.AddMinutes(30),
                false,
                "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
