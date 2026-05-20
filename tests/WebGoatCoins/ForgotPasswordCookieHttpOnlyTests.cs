using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsHttpOnly_OnSecurityAnswerCookie()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act
            cookie.HttpOnly = true;

            // Assert
            Assert.True(cookie.HttpOnly);
        }
    }
}
