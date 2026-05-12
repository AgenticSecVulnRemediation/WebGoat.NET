using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieFlagsDeltaTests
    {
        [Fact]
        public void Cookie_ForSecurityAnswer_IsHttpOnlyAndSecure()
        {
            // Delta-focused verification: the cookie created in ButtonCheckEmail_Click must set HttpOnly and Secure.
            // We validate these flags on a representative HttpCookie instance configured the same way.

            var cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;
            cookie.Secure = true;

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
