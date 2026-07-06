using System;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSetCookieSecurityFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlySecureAndSameSiteStrict()
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                "user",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                false,
                "data");

            var cookie = OWASP.WebGoat.NET.App_Code.CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
