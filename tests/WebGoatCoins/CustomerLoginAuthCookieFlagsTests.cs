using System;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

// Assumption: CustomerLogin page code-behind compiled in OWASP.WebGoat.NET.WebGoatCoins namespace.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginAuthCookieFlagsTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var page = new CustomerLogin();

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            SetField(page, "txtUserName", new TextBox { Text = "user@example.com" });
            SetField(page, "txtPassword", new TextBox { Text = "password" });
            SetField(page, "PanelError", new Panel());
            SetField(page, "labelError", new Label());

            var mockProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>();
            mockProvider.Setup(p => p.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            SetField(page, "du", mockProvider.Object);

            // Act
            var mi = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            try
            {
                mi!.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch (TargetInvocationException)
            {
                // Response.Redirect may abort request in classic ASP.NET
            }

            // Assert
            var authCookie = HttpContext.Current.Response.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
            Assert.NotNull(authCookie);
            Assert.True(authCookie.HttpOnly);
            Assert.True(authCookie.Secure);
        }

        private static void SetField(object instance, string fieldName, object value)
        {
            var f = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(instance, value);
        }
    }
}
