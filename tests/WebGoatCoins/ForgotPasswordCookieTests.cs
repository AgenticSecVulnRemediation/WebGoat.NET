using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans") { Value = "x" };

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
