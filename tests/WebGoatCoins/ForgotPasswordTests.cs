using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsCookie_HttpOnly_True()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.WebGoatCoins.ForgotPassword();

            page.GetType().GetField("PanelForgotPasswordStep2", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Panel());
            page.GetType().GetField("PanelForgotPasswordStep3", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Panel());
            page.GetType().GetField("labelQuestion", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Label());
            page.GetType().GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new TextBox { Text = "user@example.com" });

            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.GetSecurityQuestionAndAnswer("user@example.com"))
                .Returns(new[] { "Q", "A" });

            page.GetType().GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, dbProvider.Object);

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            var method = typeof(OWASP.WebGoat.NET.WebGoatCoins.ForgotPassword)
                .GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = context.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);

            dbProvider.VerifyAll();
        }
    }
}
