using Xunit;
using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Moq;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonRecoverPasswordClick_WithTamperedCookieSignature_ShowsIntegrityError()
        {
            // Arrange
            var page = new ForgotPassword();

            // Create a fake HttpContext with request/response cookies
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Inject controls needed by handler via reflection
            var txtAnswer = new System.Web.UI.WebControls.TextBox { Text = "answer" };
            var txtEmail = new System.Web.UI.WebControls.TextBox { Text = "test@example.com" };
            var labelMessage = new System.Web.UI.WebControls.Label();

            typeof(ForgotPassword).GetField("txtAnswer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, txtAnswer);
            typeof(ForgotPassword).GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, txtEmail);
            typeof(ForgotPassword).GetField("labelMessage", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, labelMessage);

            // Prepare cookie with invalid signature
            string encoded = "abc";
            context.Request.Cookies.Add(new HttpCookie("encr_sec_qu_ans", encoded + ":" + "deadbeef"));

            // Act
            var method = typeof(ForgotPassword).GetMethod("ButtonRecoverPassword_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(page, new object[] { null!, EventArgs.Empty });

            // Assert
            Assert.Contains("integrity", labelMessage.Text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
