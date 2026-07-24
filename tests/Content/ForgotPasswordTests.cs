using System;
using System.Web;
using System.Web.UI;
using Moq;
using Xunit;

// Assumption: production namespace from source file.
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsHttpOnlyCookie_ForSecurityAnswer()
        {
            // Arrange
            // We can't execute full ASP.NET pipeline; we validate the delta behavior by invoking the handler via reflection
            // and verifying it sets a cookie with HttpOnly=true on the Response.
            var page = new ForgotPassword();

            var dbProvider = new Mock<IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.GetSecurityQuestionAndAnswer(It.IsAny<string>()))
                .Returns(new[] { "question", "answer" });

            // Inject private field 'du'
            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(duField);
            duField!.SetValue(page, dbProvider.Object);

            // Setup HttpContext
            var request = new HttpRequest("", "http://localhost/", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Inject required controls/fields
            var txtEmailField = typeof(ForgotPassword).GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(txtEmailField);
            txtEmailField!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });

            var labelQuestionField = typeof(ForgotPassword).GetField("labelQuestion", BindingFlags.NonPublic | BindingFlags.Instance);
            if (labelQuestionField != null)
                labelQuestionField.SetValue(page, new System.Web.UI.WebControls.Label());

            var panel2Field = typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", BindingFlags.NonPublic | BindingFlags.Instance);
            if (panel2Field != null)
                panel2Field.SetValue(page, new System.Web.UI.WebControls.Panel());

            var panel3Field = typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", BindingFlags.NonPublic | BindingFlags.Instance);
            if (panel3Field != null)
                panel3Field.SetValue(page, new System.Web.UI.WebControls.Panel());

            // Act
            var handler = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(handler);
            handler!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);

            dbProvider.Verify(p => p.GetSecurityQuestionAndAnswer("user@example.com"), Times.Once);
        }
    }
}
