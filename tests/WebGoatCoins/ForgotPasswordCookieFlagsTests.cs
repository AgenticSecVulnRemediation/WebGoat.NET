using System;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: ForgotPassword page code-behind compiled in OWASP.WebGoat.NET.WebGoatCoins namespace.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var page = new ForgotPassword();

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            SetField(page, "txtEmail", new TextBox { Text = "user@example.com" });
            SetField(page, "labelQuestion", new Label());
            SetField(page, "PanelForgotPasswordStep2", new Panel());
            SetField(page, "PanelForgotPasswordStep3", new Panel());

            var mockProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>();
            mockProvider.Setup(p => p.GetSecurityQuestionAndAnswer(It.IsAny<string>()))
                .Returns(new[] { "Question", "Answer" });
            SetField(page, "du", mockProvider.Object);

            // Act
            var mi = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void SetField(object instance, string fieldName, object value)
        {
            var f = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(instance, value);
        }
    }
}
