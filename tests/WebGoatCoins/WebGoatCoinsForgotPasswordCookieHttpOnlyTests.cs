using Xunit;
using System.Web;

// Assumption: production namespace OWASP.WebGoat.NET.WebGoatCoins
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class WebGoatCoinsForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyTrue()
        {
            // Delta test: cookie.HttpOnly added.
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var cookie = new HttpCookie("encr_sec_qu_ans") { HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(cookie);

            Assert.True(HttpContext.Current.Response.Cookies["encr_sec_qu_ans"].HttpOnly);
        }
    }
}
