using System;
using System.Reflection;
using log4net;
using Xunit;

// Assumption: CustomerLogin page is in namespace OWASP.WebGoat.NET.WebGoatCoins and class name CustomerLogin.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            var page = new CustomerLogin();

            // Inject a fake logger to capture message
            var logField = typeof(CustomerLogin).GetField("log", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(logField);
            var fakeLog = new CapturingLog();
            logField!.SetValue(page, fakeLog);

            // Provide required fields
            SetPrivateControlText(page, "txtUserName", "user@example.com");
            SetPrivateControlText(page, "txtPassword", "SuperSecret");

            // Provide DB provider that fails login to avoid cookie/redirect
            var duField = typeof(CustomerLogin).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new FakeDbProviderValidFalse());

            SetPrivateControl(page, "PanelError", new System.Web.UI.WebControls.Panel());
            SetPrivateControl(page, "labelError", new System.Web.UI.WebControls.Label());

            // Act
            page.GetType().GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert: ensure the message doesn't contain the password
            Assert.Contains("attempted to log in", fakeLog.LastInfoMessage);
            Assert.DoesNotContain("SuperSecret", fakeLog.LastInfoMessage);
        }

        private static void SetPrivateControlText(object page, string fieldName, string text)
        {
            var tb = new System.Web.UI.WebControls.TextBox { Text = text };
            SetPrivateControl(page, fieldName, tb);
        }

        private static void SetPrivateControl(object page, string fieldName, System.Web.UI.Control control)
        {
            var field = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(page, control);
        }

        private sealed class CapturingLog : ILog
        {
            public string LastInfoMessage { get; private set; } = string.Empty;

            public void Info(object message) => LastInfoMessage = message?.ToString() ?? string.Empty;

            // Unused members for this test
            public bool IsDebugEnabled => false;
            public bool IsInfoEnabled => true;
            public bool IsWarnEnabled => false;
            public bool IsErrorEnabled => false;
            public bool IsFatalEnabled => false;
            public log4net.Core.ILogger Logger => throw new NotImplementedException();
            public void Debug(object message) { }
            public void Debug(object message, Exception exception) { }
            public void Error(object message) { }
            public void Error(object message, Exception exception) { }
            public void Fatal(object message) { }
            public void Fatal(object message, Exception exception) { }
            public void Info(object message, Exception exception) => Info(message);
            public void Warn(object message) { }
            public void Warn(object message, Exception exception) { }
            public void DebugFormat(string format, params object[] args) { }
            public void ErrorFormat(string format, params object[] args) { }
            public void FatalFormat(string format, params object[] args) { }
            public void InfoFormat(string format, params object[] args) { }
            public void WarnFormat(string format, params object[] args) { }
            public void DebugFormat(IFormatProvider provider, string format, params object[] args) { }
            public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { }
            public void FatalFormat(IFormatProvider provider, string format, params object[] args) { }
            public void InfoFormat(IFormatProvider provider, string format, params object[] args) { }
            public void WarnFormat(IFormatProvider provider, string format, params object[] args) { }
        }

        private sealed class FakeDbProviderValidFalse : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "fake";
            public bool TestConnection() => true;
            public System.Data.DataSet GetCatalogData() => new System.Data.DataSet();
            public bool RecreateGoatDb() => true;
            public bool IsValidCustomerLogin(string email, string password) => false;
            public string CustomCustomerLogin(string email, string password) => null;
            public string GetCustomerEmail(string customerNumber) => null;
            public System.Data.DataSet GetCustomerDetails(string customerNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetOffice(string city) => new System.Data.DataSet();
            public System.Data.DataSet GetComments(string productCode) => new System.Data.DataSet();
            public string AddComment(string productCode, string email, string comment) => null;
            public string UpdateCustomerPassword(int customerNumber, string password) => null;
            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "q", "a" };
            public string GetPasswordByEmail(string email) => "pw";
            public System.Data.DataSet GetUsers() => new System.Data.DataSet();
            public System.Data.DataSet GetOrders(int customerID) => new System.Data.DataSet();
            public System.Data.DataSet GetProductDetails(string productCode) => new System.Data.DataSet();
            public System.Data.DataSet GetOrderDetails(int orderNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetPayments(int customerNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetProductsAndCategories() => new System.Data.DataSet();
            public System.Data.DataSet GetProductsAndCategories(int catNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetEmailByName(string name) => new System.Data.DataSet();
            public string GetEmailByCustomerNumber(string num) => null;
            public System.Data.DataSet GetCustomerEmails(string email) => new System.Data.DataSet();
        }
    }
}
