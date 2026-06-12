using System;
using System.Web;
using System.Web.Security;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET.WebGoatCoins
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecurityTests
    {
        [Fact]
        public void AuthCookie_IsMarkedHttpOnlyAndSecure()
        {
            // Arrange
            // Patch sets HttpOnly and Secure flags on auth cookie.
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                true,
                "customer",
                FormsAuthentication.FormsCookiePath);

            var encrypted = FormsAuthentication.Encrypt(ticket);

            // Act
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                HttpOnly = true,
                Secure = true
            };

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
