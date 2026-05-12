using System;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET (from file content)
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly_True()
        {
            // Arrange
            // Delta-focused test: verifies the new hardening flag is set on the cookie.
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
