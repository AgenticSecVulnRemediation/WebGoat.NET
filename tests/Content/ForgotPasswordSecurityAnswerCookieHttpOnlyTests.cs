using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.Content
{
    public class ForgotPassword_SetsSecurityAnswerCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnlyTrue()
        {
            // Delta guard for PR #422: encr_sec_qu_ans cookie is now HttpOnly.
            // Since WebForms page events are hard to execute in a pure unit test without HttpContext wiring,
            // we assert the updated source includes the explicit HttpOnly assignment.

            var source = LoadSource();
            Assert.Contains("new HttpCookie(\"encr_sec_qu_ans\")", source);
            Assert.Contains("cookie.HttpOnly = true", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Content", "ForgotPassword.aspx.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
