using System;
using System.Web;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringProvided_SetsHttpOnlyOnCookie()
        {
            // Arrange
            var page = new HeaderInjection();

            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=abc");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = context.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
