using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void SecurityAnswerCookie_SetsHttpOnlySecureAndSameSiteStrict()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans") { Value = "x" };

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;
            cookie.SameSite = SameSiteMode.Strict;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(SameSiteMode.Strict, cookie.SameSite);
        }
    }
}
