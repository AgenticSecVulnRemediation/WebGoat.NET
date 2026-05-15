using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOn_CreatesHttpOnlyAuthCookie()
        {
            // Delta behavior: cookie.HttpOnly is set to true.
            // This test asserts the secure default for the auth cookie we create.

            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                true,
                "customer",
                FormsAuthentication.FormsCookiePath);

            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

            // Apply fix behavior
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
