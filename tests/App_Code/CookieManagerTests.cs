using System;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code (as declared in source file)

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WithTicket_SetsHttpOnlyAndSecureFlags()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow.AddMinutes(-1),
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
