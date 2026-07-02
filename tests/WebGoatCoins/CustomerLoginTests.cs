using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOn_Click_SetsAuthCookie_HttpOnly()
        {
            // Arrange
            var dbProvider = new Mock<IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.IsValidCustomerLogin("user@example.com", "pw")).Returns(true);

            // Patch: the page sets cookie.HttpOnly = true.
            // We can't run full WebForms pipeline here; instead validate the intent by executing handler via reflection and
            // using a fake HttpContext with a response cookie collection.

            var page = new CustomerLogin();

            // Inject private field 'du'
            typeof(CustomerLogin).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, dbProvider.Object);

            // Create fake controls
            typeof(CustomerLogin).GetField("txtUserName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new TextBox { Text = "user@example.com" });
            typeof(CustomerLogin).GetField("txtPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new TextBox { Text = "pw" });
            typeof(CustomerLogin).GetField("PanelError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new Panel());
            typeof(CustomerLogin).GetField("labelError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(page, new Label());

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Act
            var method = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            // The method ends with Response.Redirect which throws ThreadAbortException in classic ASP.NET.
            Assert.ThrowsAny<Exception>(() => method!.Invoke(page, new object?[] { null, EventArgs.Empty }));

            // Assert
            var cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
