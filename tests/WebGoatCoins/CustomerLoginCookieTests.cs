using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookie_HttpOnly_True()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin();

            page.GetType().GetField("PanelError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Panel());
            page.GetType().GetField("labelError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new Label());
            page.GetType().GetField("txtUserName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new TextBox { Text = "user@example.com" });
            page.GetType().GetField("txtPassword", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(page, new TextBox { Text = "pw" });

            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.IsValidCustomerLogin("user@example.com", "pw")).Returns(true);
            page.GetType().GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, dbProvider.Object);

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            var method = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetMethod("ButtonLogOn_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            // The handler calls Response.Redirect; intercept via HttpResponse.RedirectLocation after catching ThreadAbortException in ASP.NET,
            // but under this harness it may throw. We only need cookie behavior, so swallow exceptions.
            try
            {
                method!.Invoke(page, new object?[] { null, EventArgs.Empty });
            }
            catch
            {
                // ignore
            }

            // Assert
            var authCookie = context.Response.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
            Assert.NotNull(authCookie);
            Assert.True(authCookie!.HttpOnly);

            dbProvider.VerifyAll();
        }
    }
}
