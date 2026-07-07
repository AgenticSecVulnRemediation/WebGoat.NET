using System;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: default WebForms namespace matches file namespace.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringProvided_SetsCookieHttpOnly()
        {
            // Arrange
            var page = new HeaderInjection();

            // Minimal HttpContext setup.
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=test");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
