using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

// Assumption: CookieManager is in namespace OWASP.WebGoat.NET.App_Code as in source.

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsHttpOnlyOnAuthCookie()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true,
                "customer",
                FormsAuthentication.FormsCookiePath);

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
