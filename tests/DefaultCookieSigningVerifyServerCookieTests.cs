using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumption: Default.aspx.cs code-behind is compiled in OWASP.WebGoat.NET namespace.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSigningTests
    {
        [Fact]
        public void VerifyServerCookie_WithTamperedSignature_ReturnsNull()
        {
            // Arrange
            var page = new Default();

            // Build HttpContext with a tampered cookie.
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            HttpContext.Current.Request.Cookies.Add(new HttpCookie("Server", "machine|bad-signature"));

            // Act
            var mi = typeof(Default).GetMethod("VerifyServerCookie", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            var result = (string?)mi!.Invoke(page, Array.Empty<object>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void VerifyServerCookie_WithMissingDelimiter_ReturnsNull()
        {
            // Arrange
            var page = new Default();
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("Server", "no-delimiter"));

            // Act
            var mi = typeof(Default).GetMethod("VerifyServerCookie", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = (string?)mi!.Invoke(page, Array.Empty<object>());

            // Assert
            Assert.Null(result);
        }
    }
}
