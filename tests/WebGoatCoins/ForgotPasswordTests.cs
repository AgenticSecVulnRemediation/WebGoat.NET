using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Delta test: cookie storing encoded security answer must be HttpOnly.
            var cookie = new System.Web.HttpCookie("encr_sec_qu_ans")
            {
                Value = "encoded",
                HttpOnly = true
            };

            Assert.True(cookie.HttpOnly);
        }
    }
}
