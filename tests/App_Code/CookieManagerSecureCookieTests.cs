using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: CookieManager is in namespace OWASP.WebGoat.NET.App_Code as per source file.
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureCookieTests
    {
        [Fact]
        public void SetCookie_SetsSecureAndHttpOnlyToTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                "customer");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.True(cookie.Secure);
            Assert.True(cookie.HttpOnly);
        }
    }
}
