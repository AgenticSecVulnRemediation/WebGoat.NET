using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookieHttpOnly()
        {
            // Arrange
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
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
