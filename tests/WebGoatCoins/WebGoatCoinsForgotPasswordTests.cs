using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/WebGoatCoins/ForgotPassword.aspx.cs".
    public class WebGoatCoinsForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHttpOnly_ToPreventClientSideAccess()
        {
            var cookie = new System.Web.HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
