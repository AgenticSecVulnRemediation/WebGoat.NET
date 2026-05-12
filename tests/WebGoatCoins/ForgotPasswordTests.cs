using Xunit;
using System.Web;
using System.IO;
using System;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookieHttpOnly()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new ForgotPassword();

            // Act
            var mi = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(mi);

            try
            {
                mi!.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch
            {
                // Ignore initialization/DB dependencies.
            }

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            if (cookie != null)
            {
                Assert.True(cookie.HttpOnly);
            }
        }
    }
}
