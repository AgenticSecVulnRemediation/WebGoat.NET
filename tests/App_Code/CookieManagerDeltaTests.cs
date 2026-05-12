using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code and is referenced by this test project.

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerDeltaTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecure_Flags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                isPersistent: false,
                userData: "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
