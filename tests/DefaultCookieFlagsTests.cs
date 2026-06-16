using System;
using System.Web;
using System.Web.UI;
using Moq;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieFlagsTests
    {
        [Fact]
        public void Page_Load_WhenDbConfigured_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            var du = new Mock<IDbProvider>(MockBehavior.Strict);
            du.Setup(x => x.TestConnection()).Returns(true);
            du.SetupGet(x => x.Name).Returns("sqlite");

            var page = new Default();
            typeof(Default).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, du.Object);

            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            typeof(Default).GetField("lblOutput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new System.Web.UI.WebControls.Label());

            // Act
            page.GetType().GetMethod("Page_Load", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);

            du.VerifyAll();
        }
    }
}
