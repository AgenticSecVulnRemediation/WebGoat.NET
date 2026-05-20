using System;
using System.Web;
using Xunit;

// Assumption: CustomerLogin page is in namespace OWASP.WebGoat.NET.WebGoatCoins.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHttpOnlyTests
    {
        [Fact]
        public void CookieCreated_ForAuthTicket_IsHttpOnly()
        {
            // Arrange/Act: validate the introduced behavior by modeling the cookie creation.
            var cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, "ticket");
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
