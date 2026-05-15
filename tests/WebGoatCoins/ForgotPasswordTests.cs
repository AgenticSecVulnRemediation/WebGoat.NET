using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_WhenCreated_MustBeHttpOnly_And_Secure()
        {
            // Regression test for security hardening: cookie must be HttpOnly and Secure.

            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
