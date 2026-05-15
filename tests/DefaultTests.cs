using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: page class is OWASP.WebGoat.NET.Default in WebGoat/Default.aspx.cs
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenDbConnected_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            var db = new Mock<IDbProvider>();
            db.SetupGet(d => d.Name).Returns("MySql");
            db.Setup(d => d.TestConnection()).Returns(true);
            Settings.CurrentDbProvider = db.Object;

            var page = new Default();
            page.lblOutput = new Label();

            var request = new HttpRequest("", "http://localhost/", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            page.Page_Load(null, EventArgs.Empty);

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
