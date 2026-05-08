using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: production project namespace is OWASP.WebGoat.NET (from source file).
namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void Page_Load_WhenDatabaseConnects_SetsServerCookieHttpOnlyTrue()
        {
            // Arrange
            var page = new Default();

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);

            HttpContext.Current = context;

            // Ensure the response cookie collection exists.
            Assert.NotNull(HttpContext.Current.Response.Cookies);

            // Act
            // Page_Load is protected; invoke via reflection.
            var mi = typeof(Default).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
        }
    }
}
