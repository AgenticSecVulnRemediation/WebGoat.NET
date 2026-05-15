using System;
using System.IO;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieValidationTests
    {
        [Theory]
        [InlineData("abcDEF123", "abcDEF123")]
        [InlineData("abc%0d%0aInjected", "")]
        [InlineData("\r\nInjected", "")]
        public void PageLoad_WhenCookieProvided_RejectsNonAlphanumericAndSetsSecureAndHttpOnly(string input, string expected)
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/HeaderInjection.aspx", "Cookie=" + HttpUtility.UrlEncode(input));
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new HeaderInjection();

            // Act
            typeof(Page).GetMethod("ProcessRequest", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.Invoke(page, new object[] { context });

            // Assert
            var cookie = response.Cookies["UserAddedCookie"];
            Assert.NotNull(cookie);
            Assert.Equal(expected, cookie.Value);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
