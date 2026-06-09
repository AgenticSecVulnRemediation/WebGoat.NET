using System;
using System.Web.Security;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerDeltaTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnly_And_Secure()
        {
            // Delta assertion based strictly on the patch: HttpOnly and Secure are set.
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.Now, DateTime.Now.AddMinutes(1), true, "role");

            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
