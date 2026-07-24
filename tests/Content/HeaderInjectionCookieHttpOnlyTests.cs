using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumption: HeaderInjection page code-behind compiled in OWASP.WebGoat.NET namespace.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyTests
    {
        [Fact]
        public void PageLoad_WithCookieQueryString_SetsCookieHttpOnly()
        {
            // Arrange
            var page = new HeaderInjection();

            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=abc");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Act
            var mi = typeof(HeaderInjection).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
