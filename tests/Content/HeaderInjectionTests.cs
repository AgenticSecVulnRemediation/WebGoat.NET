using System;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: production code namespace is OWASP.WebGoat.NET per source file.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringProvided_SetsHttpOnlyOnIssuedCookie()
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
            var issuedCookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(issuedCookie);
            Assert.True(issuedCookie!.HttpOnly);
        }
    }
}
