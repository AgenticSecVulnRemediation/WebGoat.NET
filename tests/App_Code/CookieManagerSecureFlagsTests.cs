using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecure_AndUsesEncryptedTicketAsValue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                false,
                "userdata");

            string expectedEncrypted = FormsAuthentication.Encrypt(ticket);

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.Equal(FormsAuthentication.FormsCookieName, cookie.Name);
            Assert.Equal(expectedEncrypted, cookie.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
