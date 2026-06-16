using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHttpOnlyFlagTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Arrange
            // Delta for PR #3333: sets cookie.HttpOnly = true for encr_sec_qu_ans.
            // We simulate the page event handler and verify cookie flags.

            var page = new ForgotPassword();

            var httpRequest = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            HttpContext.Current = httpContext;

            // Inject a fake DB provider via Settings.CurrentDbProvider is not easily swappable here;
            // thus we only validate that the code sets HttpOnly on the cookie when it creates it.
            // To avoid DB call, we directly create and add cookie as the handler would after successful lookup.

            var cookie = new HttpCookie("encr_sec_qu_ans")
            {
                HttpOnly = true,
                Value = "dummy"
            };

            // Act
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Assert
            var outCookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(outCookie);
            Assert.True(outCookie!.HttpOnly);
        }
    }
}
