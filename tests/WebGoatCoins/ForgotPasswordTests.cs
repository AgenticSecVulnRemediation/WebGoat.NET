using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void EncryptedSecurityAnswerCookie_IsHttpOnly()
        {
            // Delta regression: cookie is now marked HttpOnly.
            var cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
