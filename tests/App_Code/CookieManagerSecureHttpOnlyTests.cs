using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureHttpOnlyTests
    {
        [Fact]
        public void SetCookie_SetsSecureAndHttpOnlyFlags()
        {
            // Arrange
            var ticket = new System.Web.Security.FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                isPersistent: false,
                userData: "customer");

            // Act
            var cookie = CookieManager.SetCookie(ticket, cookieId: "ignored", cookieValue: "ignored");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
