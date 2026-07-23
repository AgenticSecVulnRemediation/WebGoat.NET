using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests_SecureFlags
    {
        [Fact]
        public void SetCookie_SetsHttpOnlySecureAndSameSiteStrict()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.Now, DateTime.Now.AddMinutes(30), false, "data");

            // Act
            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
