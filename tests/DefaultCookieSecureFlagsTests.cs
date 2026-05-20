using Xunit;
using System.Web;
using System.IO;
using System.Reflection;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class Default_PageLoad_SetsCookieFlagsTests
    {
        [Fact]
        public void PageLoad_WhenDbConnects_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            // Patch adds HttpOnly and Secure flags on the "Server" cookie.
            var page = new Default();

            // Create minimal HttpContext with response.
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Force du.TestConnection() to return true by replacing private field 'du' via reflection.
            var duField = typeof(Default).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(duField);
            duField!.SetValue(page, new AlwaysConnectedDbProvider());

            // Act
            var method = typeof(Default).GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            method!.Invoke(page, new object?[] { page, System.EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private sealed class AlwaysConnectedDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "Test";
            public bool TestConnection() => true;
        }
    }
}
