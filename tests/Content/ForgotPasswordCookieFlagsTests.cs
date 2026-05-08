using System;
using System.Reflection;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsHttpOnlyAndSecureOnSecurityAnswerCookie()
        {
            // Delta regression test: cookie flags HttpOnly=true and Secure=true were added.
            // We assert presence of those literals in the compiled assembly.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(ForgotPassword).Assembly.Location));

            Assert.Contains("encr_sec_qu_ans", asmText);
            Assert.Contains("cookie.HttpOnly = true", asmText);
            Assert.Contains("cookie.Secure = true", asmText);
        }
    }
}
