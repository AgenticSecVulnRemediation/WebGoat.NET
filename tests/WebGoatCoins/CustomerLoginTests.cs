using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsFormsAuthCookieHttpOnlyAndSecure()
        {
            // Arrange
            var db = new Mock<IDbProvider>();
            db.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            Settings.CurrentDbProvider = db.Object;

            var page = new CustomerLogin
            {
                txtUserName = new TextBox { Text = "a@b.com" },
                txtPassword = new TextBox { Text = "pwd" },
                PanelError = new Panel(),
                labelError = new Label()
            };

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://localhost/", ""),
                new HttpResponse(new System.IO.StringWriter()));

            // Act
            page.ButtonLogOn_Click(null, EventArgs.Empty);

            // Assert
            var cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
