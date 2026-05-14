using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsHttpOnlyFlag()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: "user",
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.AddMinutes(30),
                isPersistent: false,
                userData: "data");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, cookieId: "ignored", cookieValue: "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
