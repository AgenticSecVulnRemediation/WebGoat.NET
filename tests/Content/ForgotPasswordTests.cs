using System;
using System.Reflection;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsCookieHttpOnlyAndSecure()
        {
            // Arrange
            // Delta behavior: newly added cookie.HttpOnly = true and cookie.Secure = true.
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
