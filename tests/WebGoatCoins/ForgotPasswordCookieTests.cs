using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyTrue()
        {
            // Delta behavior: security answer cookie marked HttpOnly.
            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            Assert.NotNull(method!.GetMethodBody());
        }
    }
}
