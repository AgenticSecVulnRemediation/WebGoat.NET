using System;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultHttpOnlyCookieTests
    {
        [Fact]
        public void PageLoad_WhenDbConfigured_ServerCookieIsHttpOnly()
        {
            // Delta: cookie.HttpOnly must be set to true.
            // We validate via a controlled HttpContext and a minimal Page lifecycle invocation.

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new Default();

            // Act: invoke Page_Load directly.
            var mi = typeof(Default).GetMethod("Page_Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
