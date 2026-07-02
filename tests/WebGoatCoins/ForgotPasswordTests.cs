using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var dbProvider = new Mock<IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.GetSecurityQuestionAndAnswer("user@example.com"))
                .Returns(new[] { "q", "a" });

            var page = new ForgotPassword();
            typeof(ForgotPassword).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, dbProvider.Object);

            typeof(ForgotPassword).GetField("txtEmail", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new TextBox { Text = "user@example.com" });
            typeof(ForgotPassword).GetField("labelQuestion", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new Label());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new Panel());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new Panel());

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Act
            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
