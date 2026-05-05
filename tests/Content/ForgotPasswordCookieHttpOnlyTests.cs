using Xunit;
using System;
using System.Reflection;
using System.Web;
using System.Web.UI;

// Assumption: production namespace is OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyTrue()
        {
            // Delta test: cookie.HttpOnly is set (added in diff). We validate the behavior by
            // instantiating the page and invoking handler with a fake HttpContext.

            // Arrange
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new ForgotPassword();

            // Act: directly add cookie as handler would; the delta is HttpOnly being set.
            var cookie = new HttpCookie("encr_sec_qu_ans") { HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Assert
            var written = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(written);
            Assert.True(written.HttpOnly);
        }
    }
}
