using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;
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

            var page = new Default { lblOutput = new Label() };

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://localhost/", ""),
                new HttpResponse(new System.IO.StringWriter()));

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
