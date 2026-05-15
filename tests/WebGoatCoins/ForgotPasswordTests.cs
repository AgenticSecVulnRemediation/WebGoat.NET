using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: page class is OWASP.WebGoat.NET.WebGoatCoins.ForgotPassword
using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly_True()
        {
            // Arrange
            var db = new Mock<IDbProvider>();
            db.Setup(d => d.GetSecurityQuestionAndAnswer(It.IsAny<string>()))
              .Returns(new[] { "Question?", "Answer" });
            Settings.CurrentDbProvider = db.Object;

            var page = new ForgotPassword();
            page.txtEmail = new TextBox { Text = "a@b.com" };
            page.labelQuestion = new Label();
            page.PanelForgotPasswordStep2 = new Panel();
            page.PanelForgotPasswordStep3 = new Panel();

            var request = new HttpRequest("", "http://localhost/", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            page.ButtonCheckEmail_Click(null, EventArgs.Empty);

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
