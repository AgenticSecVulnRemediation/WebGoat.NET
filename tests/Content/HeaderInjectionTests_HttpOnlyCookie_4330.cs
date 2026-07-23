using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests_HttpOnlyCookie
    {
        [Fact]
        public void PageLoad_WhenCookieQueryParamPresent_SetsHttpOnlyCookie()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/Content/HeaderInjection.aspx", "Cookie=test");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new HeaderInjection();

            // Act
            page.GetType().GetMethod("Page_Load", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                !.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
        }
    }
}
