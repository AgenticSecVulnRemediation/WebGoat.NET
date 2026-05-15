using System;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnly_ToPreventClientSideAccess()
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                true,
                "customer",
                FormsAuthentication.FormsCookiePath);

            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
        }
    }
}
