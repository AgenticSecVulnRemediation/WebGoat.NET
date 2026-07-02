using Xunit;
using Moq;
using log4net;
using System.Reflection;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            var page = new CustomerLogin();

            var logMock = new Mock<ILog>(MockBehavior.Strict);
            logMock.Setup(l => l.Info(It.Is<string>(msg => !msg.Contains("password") && msg.Contains("attempted"))));

            var logField = typeof(CustomerLogin).GetField("log", BindingFlags.Instance | BindingFlags.NonPublic);
            logField!.SetValue(page, logMock.Object);

            var db = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Loose);
            db.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var duField = typeof(CustomerLogin).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            duField!.SetValue(page, db.Object);

            // Set username/password textboxes
            var userField = typeof(CustomerLogin).GetField("txtUserName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var pwdField = typeof(CustomerLogin).GetField("txtPassword", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (userField != null) userField.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "u" });
            if (pwdField != null) pwdField.SetValue(page, new System.Web.UI.WebControls.TextBox { Text = "secret" });

            // Act
            var method = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            method!.Invoke(page, new object?[] { page, System.EventArgs.Empty });

            // Assert
            logMock.VerifyAll();
        }
    }
}
