using System;
using System.Reflection;
using log4net;
using Moq;
using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            var du = new Mock<IDbProvider>(MockBehavior.Strict);
            du.Setup(x => x.IsValidCustomerLogin("user@example.com", "SecretPassword123"))
              .Returns(false);

            var log = new Mock<ILog>(MockBehavior.Strict);
            log.Setup(l => l.Info(It.IsAny<object>()));

            var page = new CustomerLogin();
            typeof(CustomerLogin).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, du.Object);
            typeof(CustomerLogin).GetField("log", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, log.Object);

            typeof(CustomerLogin).GetField("txtUserName", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            typeof(CustomerLogin).GetField("txtPassword", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "SecretPassword123" });
            typeof(CustomerLogin).GetField("labelError", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Label());
            typeof(CustomerLogin).GetField("PanelError", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(page, new System.Web.UI.WebControls.Panel());

            var request = new System.Web.HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            // Act
            page.GetType().GetMethod("ButtonLogOn_Click", BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            log.Verify(l => l.Info(It.Is<object>(o => o != null && o.ToString()!.Contains("attempted to log in.") && !o.ToString()!.Contains("SecretPassword123"))), Times.Once);
            du.VerifyAll();
        }
    }
}
