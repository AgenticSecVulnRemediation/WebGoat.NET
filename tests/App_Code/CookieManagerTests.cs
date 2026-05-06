using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: Production code namespace is OWASP.WebGoat.NET.App_Code as in source file.
namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                "");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
