using System;
using Xunit;
using Moq;
using log4net;
using System.Reflection;

// Assumption: production namespace is as declared in source.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword()
        {
            // Arrange
            var page = new CustomerLogin();

            var logMock = new Mock<ILog>(MockBehavior.Strict);
            logMock.Setup(l => l.Info(It.Is<string>(msg => msg.Contains("attempted to log in.") && !msg.Contains("password", StringComparison.OrdinalIgnoreCase))));

            // Inject mocked log into instance field `log` via reflection.
            var logField = typeof(CustomerLogin).GetField("log", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(logField);
            logField!.SetValue(page, logMock.Object);

            // Provide dummy controls via reflection (only what's needed until login check returns false).
            SetTextBox(page, "txtUserName", "user@example.com");
            SetTextBox(page, "txtPassword", "SuperSecret!" );
            SetLabel(page, "labelError");
            SetPanel(page, "PanelError");

            // Also inject Db provider that always returns false to stop flow after logging.
            var dbProviderField = typeof(CustomerLogin).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(dbProviderField);
            var dbProvider = new Mock<OWASP.WebGoat.NET.App_Code.DB.IDbProvider>(MockBehavior.Strict);
            dbProvider.Setup(d => d.IsValidCustomerLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            dbProviderField!.SetValue(page, dbProvider.Object);

            // Act
            var method = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            logMock.VerifyAll();
        }

        private static void SetTextBox(object page, string fieldName, string text)
        {
            var field = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                throw new InvalidOperationException($"Field not found: {fieldName}");

            // If System.Web.UI.WebControls.TextBox isn't available in test runtime, we fake it via dynamic proxy.
            // Here we require it; if missing, project should reference System.Web.
            var tbType = Type.GetType("System.Web.UI.WebControls.TextBox, System.Web")
                        ?? Type.GetType("System.Web.UI.WebControls.TextBox");
            Assert.NotNull(tbType);
            var tb = Activator.CreateInstance(tbType!);
            tbType!.GetProperty("Text")!.SetValue(tb, text);
            field.SetValue(page, tb);
        }

        private static void SetLabel(object page, string fieldName)
        {
            var field = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                throw new InvalidOperationException($"Field not found: {fieldName}");

            var labelType = Type.GetType("System.Web.UI.WebControls.Label, System.Web")
                           ?? Type.GetType("System.Web.UI.WebControls.Label");
            Assert.NotNull(labelType);
            var label = Activator.CreateInstance(labelType!);
            field.SetValue(page, label);
        }

        private static void SetPanel(object page, string fieldName)
        {
            var field = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                throw new InvalidOperationException($"Field not found: {fieldName}");

            var panelType = Type.GetType("System.Web.UI.WebControls.Panel, System.Web")
                           ?? Type.GetType("System.Web.UI.WebControls.Panel");
            Assert.NotNull(panelType);
            var panel = Activator.CreateInstance(panelType!);
            field.SetValue(page, panel);
        }
    }
}
