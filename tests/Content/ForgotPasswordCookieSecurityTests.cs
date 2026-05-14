using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Moq;
using Xunit;

// Assumption: production page class is in namespace OWASP.WebGoat.NET (as declared in source).
namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieSecurityTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnlyAndSecure()
        {
            // Delta: cookie is now hardened with HttpOnly + Secure.

            // Arrange
            var page = new ForgotPassword();

            var httpRequest = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var httpResponse = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(httpRequest, httpResponse);
            HttpContext.Current = context;

            // Set up minimal page context
            typeof(Page).GetProperty("Context")?.SetValue(page, context);

            // Inject fake DB provider via reflection (private field 'du')
            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);

            var dbMock = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbMock.Setup(d => d.GetSecurityQuestionAndAnswer(It.IsAny<string>()))
                  .Returns(new[] { "q", "answer" });

            duField!.SetValue(page, dbMock.Object);

            // Also need txtEmail and panels/labels initialized; use reflection to set the controls if present
            var txtEmail = new System.Web.UI.WebControls.TextBox { Text = "a@b.com" };
            typeof(ForgotPassword).GetField("txtEmail", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                ?.SetValue(page, txtEmail);

            // Act
            var click = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(click);
            click!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);

            dbMock.VerifyAll();
        }
    }
}
