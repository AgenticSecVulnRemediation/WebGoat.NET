using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Moq;
using Xunit;

// Assumption: production page class is in namespace OWASP.WebGoat.NET.WebGoatCoins (as declared in source).
namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecurityTests
    {
        [Fact]
        public void ButtonLogOnClick_WhenLoginValid_SetsFormsAuthCookie_HttpOnlyAndSecure()
        {
            // Delta: auth cookie now sets HttpOnly + Secure.

            // Arrange
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin();
            typeof(Page).GetProperty("Context")?.SetValue(page, HttpContext.Current);

            // Inject fake DB provider via reflection (private field 'du')
            var duField = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);

            var dbMock = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbMock.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
            duField!.SetValue(page, dbMock.Object);

            // Set txtUserName/txtPassword
            typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetField("txtUserName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                ?.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetField("txtPassword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                ?.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "pwd" });

            // Act
            var handler = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(handler);

            // Avoid Response.Redirect throwing ThreadAbortException by setting ReturnUrl
            HttpContext.Current.Request.QueryString.Add("ReturnUrl", "/WebGoatCoins/MainPage.aspx");
            try
            {
                handler!.Invoke(page, new object?[] { page, EventArgs.Empty });
            }
            catch (TargetInvocationException tie)
            {
                // Response.Redirect may throw; ignore for cookie assertions
                _ = tie;
            }

            // Assert
            var cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);

            dbMock.VerifyAll();
        }
    }
}
