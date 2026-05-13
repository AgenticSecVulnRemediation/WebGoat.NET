using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSecurityTests
    {
        [Fact]
        public void PageLoad_WhenDbAvailable_SetsServerCookie_HttpOnly_And_Secure_And_Protected()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.Default();

            page.GetType().GetField("lblOutput", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Label());

            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.TestConnection()).Returns(true);
            dbProvider.SetupGet(p => p.Name).Returns("sqlite");
            page.GetType().GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, dbProvider.Object);

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            var method = typeof(OWASP.WebGoat.NET.Default)
                .GetMethod("Page_Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = context.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);

            // New behavior: value is base64 of MachineKey.Protect output, not plain machine name.
            Assert.True(Convert.TryFromBase64String(cookie.Value, new Span<byte>(new byte[cookie.Value.Length]), out _));

            dbProvider.VerifyAll();
        }
    }
}
