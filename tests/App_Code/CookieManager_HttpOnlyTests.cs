using System;
using OWASP.WebGoat.NET.App_Code;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManager_HttpOnlyTests
    {
        [Fact]
        public void SetCookie_SetsHttpOnlyTrue()
        {
            // Delta test for PR #629: SetCookie should mark auth cookie HttpOnly.
            var ticket = new System.Web.Security.FormsAuthenticationTicket(
                1,
                "user",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                isPersistent: false,
                userData: "data");

            var cookie = CookieManager.SetCookie(ticket, "id", "value");

            Assert.True(cookie.HttpOnly);
        }
    }
}
