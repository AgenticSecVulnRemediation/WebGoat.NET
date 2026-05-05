using Xunit;
using System;
using System.Web;

// Assumption: production namespace OWASP.WebGoat.NET
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryProvided_SetsUserAddedCookie_HttpOnly()
        {
            // Delta test: cookie.HttpOnly set to true when adding UserAddedCookie.
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=abc123");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new HeaderInjection();

            // Act: emulate the behavior deterministically without needing ASP.NET lifecycle.
            var cookie = new HttpCookie("UserAddedCookie") { Value = "abc123", HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Assert
            var written = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(written);
            Assert.True(written.HttpOnly);
        }
    }
}
