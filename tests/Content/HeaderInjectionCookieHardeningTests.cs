using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumptions:
// - Page class lives in OWASP.WebGoat.NET namespace.
// - SignAndValidateCookie is available via Some.Security.Utility imported in the page.
// This delta test focuses on the changed behavior: cookie value is passed through SignAndValidateCookie
// and cookie flags HttpOnly/Secure are set.

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjection_CookieHardeningTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringProvided_SetsHttpOnlyAndSecureOnCookie()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.HeaderInjection();

            // Create fake HttpContext
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=test");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Act
            var pageLoad = page.GetType().GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(pageLoad);
            pageLoad!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
