using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: production page class is in namespace OWASP.WebGoat.NET (as declared in source).
namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieSecurityTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryParamPresent_SetsUserAddedCookie_HttpOnly()
        {
            // Delta: cookie now sets HttpOnly=true when created from query string.

            // Arrange
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=abc");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.HeaderInjection();
            typeof(Page).GetProperty("Context")?.SetValue(page, HttpContext.Current);

            // Act
            var pageLoad = typeof(OWASP.WebGoat.NET.HeaderInjection)
                .GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(pageLoad);
            pageLoad!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.Equal("abc", cookie.Value);
        }
    }
}
