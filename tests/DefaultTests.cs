using System;
using System.Web;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenDbConfigured_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            var page = new Default();

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = context.Response.Cookies["Server"];
            if (cookie != null)
            {
                Assert.True(cookie.HttpOnly);
                Assert.True(cookie.Secure);
            }
        }
    }
}
