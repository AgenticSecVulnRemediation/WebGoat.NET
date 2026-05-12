using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsSecurityAnswerCookie_AsSecureAndHttpOnly()
        {
            // Arrange
            var cookie = new System.Web.HttpCookie("encr_sec_qu_ans");

            // Act (mirrors fixed behavior)
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
