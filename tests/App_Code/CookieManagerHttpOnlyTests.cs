using System;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    // Delta-focused test for PR 378:
    // CookieManager.SetCookie now sets HttpOnly=true on the FormsAuth cookie.
    public class CookieManagerHttpOnlyTests
    {
        [Fact]
        public void SetCookie_ShouldSet_HttpOnly_True()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                "userdata");

            // Act
            var cookie = OWASP.WebGoat.NET.App_Code.CookieManager.SetCookie(ticket, "id", "value");

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
