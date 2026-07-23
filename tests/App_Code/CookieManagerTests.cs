using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_SetsSecurityFlags_HttpOnlySecureSameSiteStrict()
        {
            // Delta behavior: HttpOnly, Secure, SameSite=Strict set on auth cookie.
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.Now, DateTime.Now.AddMinutes(5), false, "");
            var cookie = OWASP.WebGoat.NET.App_Code.CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
