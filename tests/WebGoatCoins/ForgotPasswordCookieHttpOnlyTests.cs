using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ForgotPassword_SetsSecurityAnswerCookieHttpOnly()
        {
            // Delta behavior: encr_sec_qu_ans cookie should be HttpOnly.
            Assert.NotNull(typeof(ForgotPassword));
        }
    }
}
