using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: Page class is in OWASP.WebGoat.NET namespace.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void PageLoad_WithCookieQueryString_AddsHttpOnlyCookie()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=test");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new HeaderInjection();

            // Act
            page.ProcessRequest(context);

            // Assert
            var addedCookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(addedCookie);
            Assert.True(addedCookie.HttpOnly);
        }
    }
}
