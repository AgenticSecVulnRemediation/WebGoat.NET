using System;
using System.IO;
using System.Web;
using Xunit;

// Assumption: Page class is in OWASP.WebGoat.NET namespace.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieValidationTests
    {
        [Fact]
        public void PageLoad_WithInvalidCookieValue_ResetsCookieValueAndSetsSecureFlags()
        {
            // Arrange: inject a value that fails ^[a-zA-Z0-9]*$
            var request = new HttpRequest("", "https://localhost/HeaderInjection.aspx", "Cookie=bad%0d%0aSet-Cookie:x");
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new HeaderInjection();

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.Equal(string.Empty, cookie.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        [Fact]
        public void PageLoad_WithValidCookieValue_PreservesCookieValueAndSetsSecureFlags()
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/HeaderInjection.aspx", "Cookie=Abc123");
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new HeaderInjection();

            // Act
            page.ProcessRequest(context);

            // Assert
            var cookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.Equal("Abc123", cookie.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
