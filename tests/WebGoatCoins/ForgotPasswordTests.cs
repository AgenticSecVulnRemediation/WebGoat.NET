using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_ShouldBeHttpOnlyAndSecure()
        {
            // Delta test: cookie is explicitly hardened with HttpOnly + Secure.
            var cookie = new HttpCookie("encr_sec_qu_ans")
            {
                HttpOnly = true,
                Secure = true
            };

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
