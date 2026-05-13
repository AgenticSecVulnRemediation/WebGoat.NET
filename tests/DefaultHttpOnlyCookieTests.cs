using System;
using System.Web;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenDbConfigured_AddsServerCookieAsHttpOnly()
        {
            // Arrange
            // This is a delta test for the security fix: the "Server" cookie must be HttpOnly.
            // We simulate HttpContext and invoke Page_Load via reflection.
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new Default();

            // Inject a fake Session via HttpContext
            // (ASP.NET creates it lazily; this call ensures Session is available.)
            var _ = HttpContext.Current.Session;

            // Act
            page.GetType().GetMethod("Page_Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
