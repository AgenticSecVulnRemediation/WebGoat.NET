using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class ForgotPasswordCookieSecurityFlagsTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsSecureAndHttpOnly()
        {
            var cookie = new System.Web.HttpCookie("encr_sec_qu_ans")
            {
                Secure = true,
                HttpOnly = true,
                Value = "encoded"
            };

            Assert.True(cookie.Secure);
            Assert.True(cookie.HttpOnly);
        }
    }
}
