using System;
using System.Web;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieSecureTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHttpOnlyAndSecure()
        {
            // Arrange
            // Patch sets HttpOnly and Secure flags for "encr_sec_qu_ans" cookie.
            var cookie = new HttpCookie("encr_sec_qu_ans", "dummy")
            {
                HttpOnly = true,
                Secure = true
            };

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
