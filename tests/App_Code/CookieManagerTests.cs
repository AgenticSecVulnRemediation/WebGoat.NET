using System;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

// Assumption: source project exposes CookieManager in OWASP.WebGoat.NET.App_Code namespace.

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesCookieWithHttpOnlyAndSecureFlagsEnabled()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "data");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
