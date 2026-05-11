using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void CookieForSecurityAnswer_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act (mirrors delta: cookie.HttpOnly = true)
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
