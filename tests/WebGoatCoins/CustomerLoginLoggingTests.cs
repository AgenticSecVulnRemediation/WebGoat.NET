using System;
using System.Web;
using System.Web.UI.WebControls;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword()
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
                ?.SetValue(page, new TextBox { Text = "supersecret" });

            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(p => p.IsValidCustomerLogin("user@example.com", "supersecret")).Returns(false);
            page.GetType().GetField("du", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, dbProvider.Object);

            // Swap logger field 'log' with a mock to capture message.
            var logMock = new Mock<log4net.ILog>(MockBehavior.Strict);
            logMock.Setup(l => l.Info(It.IsAny<object>()));
            page.GetType().GetField("log", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(page, logMock.Object);

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            var method = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin)
                .GetMethod("ButtonLogOn_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            logMock.Verify(l => l.Info(It.Is<object>(o =>
                o != null && o.ToString()!.Contains("attempted to log in.") && !o.ToString()!.Contains("supersecret"))), Times.Once);

            dbProvider.VerifyAll();
        }
    }
}
