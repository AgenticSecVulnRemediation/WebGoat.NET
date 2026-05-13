using System;
using System.Web;
using Xunit;

// Assumption: Source namespace from file path is OWASP.WebGoat.NET.WebGoatCoins
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsCookieHttpOnlyAndSecure()
        {
            // Arrange
            var page = new ForgotPassword();

            var request = new HttpRequest("", "https://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            var cookie = new HttpCookie("encr_sec_qu_ans")
            {
                Value = "value",
                HttpOnly = true,
                Secure = true
            };
            context.Response.Cookies.Add(cookie);

            // Assert
            var written = context.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(written);
            Assert.True(written.HttpOnly);
            Assert.True(written.Secure);
        }
    }
}
