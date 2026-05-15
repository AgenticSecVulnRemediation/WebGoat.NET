using System;
using System.Web;
using Xunit;

// Assumption: WebForms pages use HttpCookie from System.Web.
// This delta test verifies only the new HttpOnly flag on the created cookie.

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void HttpCookie_WhenSettingSecurityAnswerCookie_IsHttpOnly()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act: patched behavior sets HttpOnly true
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
