using Xunit;
using Moq;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Reflection;
using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieFlagsTests
    {
        [Fact]
        public void ButtonLogOn_Click_SetsAuthCookie_HttpOnly_And_Secure()
        {
            // Arrange
            // We run the handler against a minimal HttpContext and fake DbProvider.
            var page = new CustomerLogin();

            var httpRequest = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var httpResponse = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(httpRequest, httpResponse);
            HttpContext.Current = context;

            // Fake the db provider to return valid login.
            var dbMock = new Mock<IDbProvider>(MockBehavior.Strict);
            dbMock.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Inject Settings.CurrentDbProvider via reflection if needed.
            // Note: if Settings.CurrentDbProvider is not settable, this test will still validate that when a cookie is added
            // it must include the flags; in that case, we skip by asserting a specific exception type.
            TrySetCurrentDbProvider(dbMock.Object);

            // Wire up minimal controls via reflection (txtUserName/txtPassword).
            SetTextBox(page, "txtUserName", "user@example.com");
            SetTextBox(page, "txtPassword", "password");

            // Act
            var ex = Record.Exception(() => InvokeNonPublic(page, "ButtonLogOn_Click", page, EventArgs.Empty));

            // Assert
            Assert.Null(ex);
            var cookie = context.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void TrySetCurrentDbProvider(IDbProvider provider)
        {
            var settingsType = Type.GetType("OWASP.WebGoat.NET.App_Code.Settings, OWASP.WebGoat.NET");
            if (settingsType == null) return;

            var prop = settingsType.GetProperty("CurrentDbProvider", BindingFlags.Public | BindingFlags.Static);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(null, provider);
                return;
            }

            var field = settingsType.GetField("CurrentDbProvider", BindingFlags.Public | BindingFlags.Static)
                        ?? settingsType.GetField("_currentDbProvider", BindingFlags.NonPublic | BindingFlags.Static);
            field?.SetValue(null, provider);
        }

        private static void SetTextBox(object page, string fieldName, string value)
        {
            var pageType = page.GetType();
            var field = pageType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null) return;

            var tb = field.GetValue(page);
            var textProp = tb?.GetType().GetProperty("Text");
            textProp?.SetValue(tb, value);
        }

        private static void InvokeNonPublic(object target, string methodName, params object[] args)
        {
            var mi = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(target, args);
        }
    }
}
