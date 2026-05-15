using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: page class is OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin
using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookieHttpOnlyAndSecure()
        {
            // Arrange
            var db = new Mock<IDbProvider>();
            db.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            Settings.CurrentDbProvider = db.Object;

            var page = new CustomerLogin();
            page.txtUserName = new TextBox { Text = "a@b.com" };
            page.txtPassword = new TextBox { Text = "pwd" };
            page.PanelError = new Panel();
            page.labelError = new Label();

            var request = new HttpRequest("", "https://localhost/", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

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
