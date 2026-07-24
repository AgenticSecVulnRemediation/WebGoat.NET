using System;
using System.Web.Security;
using Xunit;

using OWASP.WebGoat.NET.App_Code;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerSecureFlagsTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyAndSecureFlags()
        {
            // PR 4602: cookie hardened to HttpOnly + Secure.
            var ticket = new FormsAuthenticationTicket(1, "user", DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30), false, "data");

            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
