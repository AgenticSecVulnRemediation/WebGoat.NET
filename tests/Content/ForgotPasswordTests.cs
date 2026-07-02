using Xunit;
using Moq;
using System.Web;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            // Create a fake HttpContext to capture response cookies
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Mock db provider to return question/answer
            var db = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Loose);
            db.Setup(d => d.GetSecurityQuestionAndAnswer(It.IsAny<string>())).Returns(new[] { "q", "a" });

            // inject via reflection
            var field = typeof(ForgotPassword).GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field!.SetValue(page, db.Object);

            // also set txtEmail field via reflection (WebForms control)
            var txtEmailField = typeof(ForgotPassword).GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            if (txtEmailField != null)
            {
                var tb = new System.Web.UI.WebControls.TextBox { Text = "user@example.com" };
                txtEmailField.SetValue(page, tb);
            }

            // Act
            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(page, new object?[] { page, System.EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
