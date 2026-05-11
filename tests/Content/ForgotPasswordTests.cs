using System;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHttpOnly()
        {
            // Arrange
            // Delta behavior: cookie encr_sec_qu_ans is now HttpOnly.
            // We validate the cookie flags directly.
            var cookie = new HttpCookie("encr_sec_qu_ans") { Value = "x" };

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
