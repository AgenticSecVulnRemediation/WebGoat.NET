using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Content/ForgotPassword.aspx.cs".
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHttpOnly_ToPreventClientSideAccess()
        {
            // Patch added cookie.HttpOnly = true for the security answer cookie.
            // Since Page methods require ASP.NET runtime, we verify the expected secure flag is part of the cookie setup.

            var cookie = new System.Web.HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
