using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using Moq;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieSecurityTests
    {
        [Fact]
        public void Page_Load_WhenDbConnected_SetsServerCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var db = new Mock<IDbProvider>(MockBehavior.Strict);
            db.Setup(d => d.TestConnection()).Returns(true);
            db.SetupGet(d => d.Name).Returns("TestDb");

            // Force Settings.CurrentDbProvider to our stub (best-effort: use reflection for private setter/field).
            var settingsType = typeof(OWASP.WebGoat.NET.App_Code.Settings);
            var prop = settingsType.GetProperty("CurrentDbProvider");
            Assert.NotNull(prop);

            // Some projects expose CurrentDbProvider as settable; if not, this test will fail fast and signal need for adjustment.
            prop!.SetValue(null, db.Object);

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new Default();

            // Act
            var pageLoad = page.GetType().GetMethod("Page_Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(pageLoad);
            pageLoad!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
