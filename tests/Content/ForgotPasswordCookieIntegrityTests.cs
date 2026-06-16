using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using Moq;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieIntegrityTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsHttpOnlySecureAndSignsCookieValue()
        {
            // Arrange
            // Delta test: cookie now includes HMAC signature and is marked HttpOnly+Secure.

            var du = new Mock<IDbProvider>(MockBehavior.Strict);
            du.Setup(x => x.GetSecurityQuestionAndAnswer("user@example.com"))
              .Returns(new[] { "q", "answer" });

            // Provide CookieSecretKey setting.
            var nvc = new NameValueCollection { { "CookieSecretKey", "unit-test-secret" } };
            typeof(ConfigurationManager).GetField("s_appSettings", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, nvc);

            var page = new ForgotPassword();
            typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, du.Object);

            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var responseWriter = new System.IO.StringWriter();
            var response = new HttpResponse(responseWriter);
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // wire up controls
            typeof(ForgotPassword).GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            typeof(ForgotPassword).GetField("labelQuestion", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Label());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());

            // Act
            page.GetType().GetMethod("ButtonCheckEmail_Click", BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);

            // value must be "encoded:signature"
            var parts = cookie.Value.Split(':');
            Assert.Equal(2, parts.Length);
            Assert.False(string.IsNullOrWhiteSpace(parts[0]));
            Assert.False(string.IsNullOrWhiteSpace(parts[1]));

            // signature should match HMACSHA256 over encodedValue.
            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes("unit-test-secret"));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(parts[0]));
            var expectedSig = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            Assert.Equal(expectedSig, parts[1]);

            du.VerifyAll();
        }

        [Fact]
        public void ButtonRecoverPassword_Click_WithTamperedCookieSignature_ShowsErrorMessage()
        {
            // Arrange
            // Delta test: tampered cookie must fail integrity check and be handled via exception path.

            var du = new Mock<IDbProvider>(MockBehavior.Strict);
            du.Setup(x => x.GetPasswordByEmail("user@example.com")).Returns("pw");

            var nvc = new NameValueCollection { { "CookieSecretKey", "unit-test-secret" } };
            typeof(ConfigurationManager).GetField("s_appSettings", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, nvc);

            var page = new ForgotPassword();
            typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, du.Object);

            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            request.Cookies.Add(new HttpCookie("encr_sec_qu_ans", "encodedValue:WRONGSIG"));
            var responseWriter = new System.IO.StringWriter();
            var response = new HttpResponse(responseWriter);
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var label = new System.Web.UI.WebControls.Label();
            typeof(ForgotPassword).GetField("labelMessage", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, label);
            typeof(ForgotPassword).GetField("txtAnswer", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "answer" });
            typeof(ForgotPassword).GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep1", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());
            typeof(ForgotPassword).GetField("labelPassword", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Label());

            // Act
            page.GetType().GetMethod("ButtonRecoverPassword_Click", BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            Assert.Contains("Cookie integrity check failed", label.Text);

            // In this failure path GetPasswordByEmail should not be called.
            du.Verify(x => x.GetPasswordByEmail(It.IsAny<string>()), Times.Never);
        }
    }
}
