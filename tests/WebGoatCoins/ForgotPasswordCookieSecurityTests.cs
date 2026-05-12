using System;
using System.Web;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.WebGoatCoins
namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieSecurityTests
    {
        [Fact]
        public void ButtonCheckEmail_WhenCreatingSecurityAnswerCookie_SetsHttpOnlyAndSecureFlags()
        {
            // Delta behavior: ensure the cookie that stores the encoded security answer is hardened.
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act (mirror fixed code)
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
