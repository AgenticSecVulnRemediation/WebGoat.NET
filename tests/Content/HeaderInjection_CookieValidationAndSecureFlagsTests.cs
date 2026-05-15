using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjection_CookieValidationAndSecureFlagsTests
    {
        [Fact]
        public void PageLoad_WhenCookieContainsNonAlphanumeric_ResetsCookieValue_AndSetsHttpOnlyAndSecure()
        {
            // Delta test for PR #635: cookie value validation + HttpOnly + Secure.

            var request = new System.Web.HttpRequest("", "http://localhost/Content/HeaderInjection.aspx", "Cookie=bad%0d%0aSet-Cookie:evil=1");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            var page = new HeaderInjection();

            InvokePageLoad(page);

            var cookie = System.Web.HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.Equal(string.Empty, cookie!.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        [Fact]
        public void PageLoad_WhenCookieIsAlphanumeric_PreservesCookieValue_AndSetsHttpOnlyAndSecure()
        {
            var request = new System.Web.HttpRequest("", "http://localhost/Content/HeaderInjection.aspx", "Cookie=abc123");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            var page = new HeaderInjection();

            InvokePageLoad(page);

            var cookie = System.Web.HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.Equal("abc123", cookie!.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void InvokePageLoad(HeaderInjection page)
        {
            var m = typeof(HeaderInjection).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(m);
            m!.Invoke(page, new object?[] { page, EventArgs.Empty });
        }
    }
}
