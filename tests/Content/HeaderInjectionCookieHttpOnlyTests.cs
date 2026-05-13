using System;
using System.IO;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringProvided_SetsUserAddedCookieAsHttpOnly()
        {
            // Arrange
            var page = new HeaderInjection();

            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=test");
            var responseWriter = new StringWriter();
            var response = new HttpResponse(responseWriter);
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
