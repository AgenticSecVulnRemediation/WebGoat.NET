using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjection_CookieFlagsTests
    {
        [Fact]
        public void PageLoad_WhenCookieQueryStringPresent_SetsHttpOnlyOnCookie()
        {
            // Delta test for PR #640: cookie.HttpOnly should be set.
            // WebForms Page lifecycle is heavy to run; validate by executing Page_Load via reflection
            // after injecting a fake HttpContext.

            var request = new System.Web.HttpRequest("", "http://localhost/Content/HeaderInjection.aspx", "Cookie=abc");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            var page = new HeaderInjection();

            // Act
            InvokePageLoad(page);

            // Assert
            var cookie = System.Web.HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        private static void InvokePageLoad(HeaderInjection page)
        {
            var m = typeof(HeaderInjection).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(m);
            m!.Invoke(page, new object?[] { page, EventArgs.Empty });
        }
    }
}
