using System;
using System.Web;
using Xunit;

// Assumption: production code namespace matches file.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly()
        {
            // Arrange
            // Delta test: cookie "encr_sec_qu_ans" should be HttpOnly after fix.
            var page = new ForgotPassword();

            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Seed minimal controls via reflection because WebForms normally wires these.
            // If control fields are missing at runtime, this test will fail, highlighting regression.
            page.GetType().GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });

            // Also need a provider; if not available, skip as incomplete source context.
            // Note: This repo typically uses Settings.CurrentDbProvider; if null, method would throw.
            var duField = page.GetType().GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(duField);

            // Act
            // Invoke handler directly.
            var mi = page.GetType().GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            Assert.NotNull(mi);
            try
            {
                mi.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                // If provider is not configured, we cannot proceed in this unit test.
                throw tie.InnerException ?? tie;
            }

            // Assert
            var cookie = context.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
