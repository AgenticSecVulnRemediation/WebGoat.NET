using System;
using System.Web;
using System.Web.UI;
using Moq;
using Xunit;

// NOTE: Namespace inferred from source file namespace `OWASP.WebGoat.NET`.
namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests_CookieHardening
    {
        [Fact]
        public void Page_Load_WhenDbConnected_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            // Patch change: cookie.HttpOnly=true and cookie.Secure=true.
            var page = new Default();

            // Create HttpContext with writable cookies collection
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Inject du (private field) via reflection to force TestConnection() == true
            var duField = typeof(Default).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(duField);

            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.TestConnection()).Returns(true);
            dbProvider.SetupGet(p => p.Name).Returns("mysql");

            duField!.SetValue(page, dbProvider.Object);

            // Also provide Server for MachineName access
            page.GetType().GetProperty("Server")?.SetValue(page, new HttpServerUtility(context));

            // Act
            page.GetType().GetMethod("Page_Load", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                !.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = context.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);

            dbProvider.VerifyAll();
        }
    }
}
