using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieSecureFlagsTests
    {
        [Fact]
        public void WebGoatCoinsForgotPassword_SetsSecurityAnswerCookie_HttpOnly_And_Secure()
        {
            var src = System.IO.File.ReadAllText("WebGoat/WebGoatCoins/ForgotPassword.aspx.cs");

            Assert.Contains("cookie.HttpOnly = true", src);
            Assert.Contains("cookie.Secure = true", src);
        }
    }
}
