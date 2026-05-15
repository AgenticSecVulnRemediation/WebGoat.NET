using System;
using System.Web.Security;
using Xunit;

// Assumption: CookieManager is in OWASP.WebGoat.NET.App_Code namespace as in source file.
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureFlags_OnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(10),
                false,
                "data");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.False(string.IsNullOrWhiteSpace(cookie.Value));
        }
    }
}
