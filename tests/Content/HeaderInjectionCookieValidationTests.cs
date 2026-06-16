using System;
using System.Reflection;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieValidationTests
    {
        [Fact]
        public void PageLoad_InvalidCookieValue_ThrowsArgumentException()
        {
            // Arrange: cookie contains CRLF injection attempt
            var request = new HttpRequest("", "http://localhost/HeaderInjection.aspx", "Cookie=abc%0d%0aSet-Cookie%3Aevil%3D1");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.HeaderInjection();

            // Act + Assert
            var method = typeof(OWASP.WebGoat.NET.HeaderInjection).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(page, new object[] { page, EventArgs.Empty }));
            Assert.IsType<ArgumentException>(ex.InnerException);
        }

        [Fact]
        public void PageLoad_ValidCookieValue_SetsSecureAndHttpOnlyCookie()
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/HeaderInjection.aspx", "Cookie=Abc123");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.HeaderInjection();

            // Act
            var method = typeof(OWASP.WebGoat.NET.HeaderInjection).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            method!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal("Abc123", cookie.Value);
        }
    }
}
