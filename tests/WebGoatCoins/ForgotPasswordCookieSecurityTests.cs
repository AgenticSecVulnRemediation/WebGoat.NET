using System;
using System.Web;
using System.Web.UI;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieSecurityTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var db = new Mock<IDbProvider>(MockBehavior.Strict);
            db.Setup(d => d.GetSecurityQuestionAndAnswer(It.IsAny<string>())).Returns(new[] { "Question", "Answer" });

            var settingsType = typeof(OWASP.WebGoat.NET.App_Code.Settings);
            var prop = settingsType.GetProperty("CurrentDbProvider");
            Assert.NotNull(prop);
            prop!.SetValue(null, db.Object);

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new ForgotPassword();

            // Provide minimal controls used by handler.
            page.GetType().GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "a@b.com" });
            page.GetType().GetField("txtAnswer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "Answer" });
            page.GetType().GetField("labelQuestion", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(page, new System.Web.UI.WebControls.Label());
            page.GetType().GetField("PanelForgotPasswordStep2", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(page, new System.Web.UI.WebControls.Panel());
            page.GetType().GetField("PanelForgotPasswordStep3", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(page, new System.Web.UI.WebControls.Panel());

            var handler = page.GetType().GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(handler);

            // Act
            handler!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
