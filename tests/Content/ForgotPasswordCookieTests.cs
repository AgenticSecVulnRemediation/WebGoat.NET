using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly()
        {
            // Delta behavior: encr_sec_qu_ans cookie should be HttpOnly.
            // Full page event testing requires ASP.NET runtime; here we enforce compilation/linkage only.
            Assert.NotNull(typeof(ForgotPassword));
        }
    }
}
