using System;
using System.Web.Security;
using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_CreatesHttpOnlyAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                isPersistent: false,
                userData: string.Empty);

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignoredId", "ignoredValue");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
