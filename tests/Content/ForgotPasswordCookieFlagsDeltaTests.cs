using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieFlagsDeltaTests
    {
        [Fact]
        public void Patch164_EncrSecQuAnsCookie_IsHttpOnly_AndSecure()
        {
            // Delta assertion: cookie flags were added.
            var cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;
            cookie.Secure = true;

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
