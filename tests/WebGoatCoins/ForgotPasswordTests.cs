using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.WebGoatCoins;

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

            var page = new ForgotPassword
            {
                txtEmail = new TextBox { Text = "a@b.com" },
                labelQuestion = new Label(),
                PanelForgotPasswordStep2 = new Panel(),
                PanelForgotPasswordStep3 = new Panel()
            };

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost/", ""),
                new HttpResponse(new System.IO.StringWriter()));

            // Act
            page.ButtonCheckEmail_Click(null, EventArgs.Empty);

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
