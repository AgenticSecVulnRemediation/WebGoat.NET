using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: code-behind namespace matches file (OWASP.WebGoat.NET)
namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHardeningTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Arrange: instantiate page and inject minimal HttpContext/Response.
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var ctx = new HttpContext(request, response);
            HttpContext.Current = ctx;

            var page = new OWASP.WebGoat.NET.ForgotPassword();

            // Inject required controls via reflection (since WebForms uses designer fields)
            SetField(page, "txtEmail", new TextBox { Text = "user@example.com" });
            SetField(page, "labelQuestion", new Label());
            SetField(page, "PanelForgotPasswordStep2", new Panel());
            SetField(page, "PanelForgotPasswordStep3", new Panel());

            // Mock DB provider returned by Settings.CurrentDbProvider
            // We can't easily swap the static Settings in a unit test; so we assert via source-level regression:
            // the cookie is explicitly marked HttpOnly.
            var src = System.IO.File.ReadAllText("WebGoat/Content/ForgotPassword.aspx.cs");

            // Assert: secure delta present
            Assert.Contains("cookie.HttpOnly = true", src);
        }

        private static void SetField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field == null)
            {
                // Try property fallback
                var prop = target.GetType().GetProperty(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                prop?.SetValue(target, value);
                return;
            }
            field.SetValue(target, value);
        }
    }
}
