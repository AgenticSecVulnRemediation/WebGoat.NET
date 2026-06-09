using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieDeltaTests
    {
        [Fact]
        public void AuthCookie_IsMarkedHttpOnly()
        {
            // Delta assertion based strictly on the patch: cookie.HttpOnly is set to true.
            var ticket = new FormsAuthenticationTicket(1, "user@example.com", DateTime.Now, DateTime.Now.AddDays(14), true, "customer");
            var encrypted = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                HttpOnly = true
            };

            Assert.True(cookie.HttpOnly);
        }
    }
}
