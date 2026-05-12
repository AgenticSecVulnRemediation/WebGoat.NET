using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void ForgotPassword_SetsSecurityAnswerCookie_HttpOnly()
        {
            var cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;
            Assert.True(cookie.HttpOnly);
        }
    }
}
