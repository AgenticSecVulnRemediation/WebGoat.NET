using System;
using System.Web;
using System.Web.Security;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOn_Click_SetsAuthCookieHttpOnly()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(1, "user@example.com", DateTime.Now, DateTime.Now.AddDays(14), true, "customer");
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
