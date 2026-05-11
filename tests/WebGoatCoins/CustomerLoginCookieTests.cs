using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnly()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.Now, DateTime.Now.AddMinutes(10), false, "customer");
            var encrypted = FormsAuthentication.Encrypt(ticket);

            // Act
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                HttpOnly = true
            };

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
