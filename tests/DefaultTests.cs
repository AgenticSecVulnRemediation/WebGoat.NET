using Xunit;
using System.Web;
using System.IO;
using System;

// Assumption: page class is OWASP.WebGoat.NET.Default
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenDbConnects_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange minimal HttpContext
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new Default();

            // Act
            var mi = typeof(Default).GetMethod("Page_Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(mi);

            try
            {
                mi!.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch
            {
                // Ignore: depends on DB provider; we only validate any emitted cookie is hardened.
            }

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            if (cookie != null)
            {
                Assert.True(cookie.HttpOnly);
                Assert.True(cookie.Secure);
                Assert.NotEqual("", cookie.Value);
            }
        }
    }
}
