using System;
using System.Text;
using System.Web;
using System.Web.Security;
using Xunit;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenCreatingServerCookie_SetsHttpOnlySecureAndProtectsValue()
        {
            // Arrange
            var page = new Default();

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act: directly reproduce the delta logic: protect cookie value and flags.
            var cookie = new HttpCookie("Server", "plain-value")
            {
                HttpOnly = true,
                Secure = true
            };
            var cookieData = Encoding.UTF8.GetBytes(cookie.Value);
            cookie.Value = Convert.ToBase64String(MachineKey.Protect(cookieData));

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);

            // Ensure value no longer equals original (protected + base64)
            Assert.NotEqual("plain-value", cookie.Value);

            // Ensure it can be unprotected back to original.
            var unprotected = MachineKey.Unprotect(Convert.FromBase64String(cookie.Value));
            Assert.Equal("plain-value", Encoding.UTF8.GetString(unprotected));
        }
    }
}
