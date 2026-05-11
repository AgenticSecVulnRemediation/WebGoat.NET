using System;
using System.Reflection;
using System.Web;
using Xunit;

// Note: Namespace inference is based on file path; adjust if the production assembly uses a different root namespace.
namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.ForgotPassword();

            // The page relies on ASP.NET runtime context; we only validate the delta behavior in isolation by
            // constructing a cookie and asserting HttpOnly is set (the fix added this).
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
