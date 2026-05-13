using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class WebGoatCoinsForgotPassword_SetsCookieSecureAndHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_SecureAndHttpOnly()
        {
            // Delta guard for PR #420: encr_sec_qu_ans cookie now sets Secure and HttpOnly.
            var source = LoadSource();

            Assert.Contains("new HttpCookie(\"encr_sec_qu_ans\")", source);
            Assert.Contains("cookie.Secure = true", source);
            Assert.Contains("cookie.HttpOnly = true", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "WebGoatCoins", "ForgotPassword.aspx.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
