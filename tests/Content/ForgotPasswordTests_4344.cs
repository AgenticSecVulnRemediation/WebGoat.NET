using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Moq;
using Xunit;

// Assumption: Source namespace is OWASP.WebGoat.NET
namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsHttpOnlyOnSecurityAnswerCookie()
        {
            // Arrange
            var page = new ForgotPassword();

            // Inject a fake IDbProvider into private field 'du'
            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance);
            var dbProviderMock = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProviderMock.Setup(p => p.GetSecurityQuestionAndAnswer("user@example.com"))
                .Returns(new[] { "Question", "Answer" });
            dbProviderMock.Setup(p => p.GetPasswordByEmail(It.IsAny<string>())).Returns("pw");
            duField!.SetValue(page, dbProviderMock.Object);

            // Minimal HttpContext to capture cookies added.
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Wire up required controls via reflection
            typeof(ForgotPassword).GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            typeof(ForgotPassword).GetField("labelQuestion", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(page, new System.Web.UI.WebControls.Label());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(page, new System.Web.UI.WebControls.Panel());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(page, new System.Web.UI.WebControls.Panel());

            // Act
            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            method!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
