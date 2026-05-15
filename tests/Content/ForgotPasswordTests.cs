using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: page class is OWASP.WebGoat.NET.ForgotPassword in WebGoat/Content/ForgotPassword.aspx.cs
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.Tests
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

            // Replace Settings.CurrentDbProvider via reflection if it's static; otherwise this test may need adjustment.
            // We assume Settings.CurrentDbProvider is settable for test.
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
