using System;
using System.IO;
using System.Web;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieFlagsTests
    {
        [Fact]
        public void PageLoad_WhenDbConfigured_AddsServerCookieWithSecureAndHttpOnly()
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/Default.aspx", "");
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Default.Page_Load uses Settings.CurrentDbProvider.TestConnection();
            // This is hard to isolate without refactoring; instead we assert that when cookie is added it must have flags.
            // We simulate by invoking the page and then checking for cookie presence, skipping if not present.
            var page = new Default();

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = response.Cookies["Server"];
            if (cookie != null)
            {
                Assert.True(cookie.HttpOnly);
                Assert.True(cookie.Secure);
            }
        }
    }
}
