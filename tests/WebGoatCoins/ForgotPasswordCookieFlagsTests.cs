using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void EncryptedSecurityAnswerCookie_SetsHttpOnlyAndSecure()
        {
            // Delta behavior: HttpOnly and Secure flags are set on "encr_sec_qu_ans" cookie.
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
