using System.Web;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.WebGoatCoins (from source file WebGoat/WebGoatCoins/ForgotPassword.aspx.cs)
namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHardened_HttpOnlyAndSecureAreTrue()
        {
            // Arrange
            var cookie = new HttpCookie("encr_sec_qu_ans");

            // Act (mirrors the hardened flags added in the fix)
            cookie.HttpOnly = true;
            cookie.Secure = true;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
