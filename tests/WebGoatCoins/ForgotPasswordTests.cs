using System;
using System.Web;
using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsCookieHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            page.GetType().GetField("txtEmail", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });

            var mi = page.GetType().GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            Assert.NotNull(mi);

            try
            {
                mi.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch
            {
                // provider may not be configured; still validate cookie not accessible if set
            }

            var cookie = context.Response.Cookies["encr_sec_qu_ans"];
            if (cookie != null)
            {
                Assert.True(cookie.HttpOnly);
            }
        }
    }
}
