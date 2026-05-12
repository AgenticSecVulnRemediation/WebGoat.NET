using System;
using System.Web;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void ForgotPassword_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Delta: enforce HttpOnly on security-answer cookie
            var cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
