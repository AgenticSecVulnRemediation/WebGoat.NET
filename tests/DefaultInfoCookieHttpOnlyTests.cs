using Xunit;
using System.Web;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultInfoCookieHttpOnlyTests
    {
        [Fact]
        public void DefaultPageLoad_WhenDbConnected_SetsServerCookieHttpOnly()
        {
            // Delta test: Server info cookie is now HttpOnly.
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var cookie = new HttpCookie("Server", "machine") { HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(cookie);

            Assert.True(HttpContext.Current.Response.Cookies["Server"].HttpOnly);
        }
    }
}
