using System;
using System.Reflection;
using System.Web;
using OWASP.WebGoat.NET.WebGoatCoins;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var page = new ForgotPassword();

            var request = new HttpRequest("", "https://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(duField);
            duField!.SetValue(page, new FakeDbProvider());

            // Set txtEmail control via reflection (auto-generated fields in WebForms)
            SetTextBox(page, "txtEmail", "user@example.com");
            SetLabel(page, "labelQuestion");
            SetPanel(page, "PanelForgotPasswordStep2");
            SetPanel(page, "PanelForgotPasswordStep3");

            var handler = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(handler);

            // Act
            handler!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void SetTextBox(object page, string fieldName, string value)
        {
            var f = page.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null) return; // tolerate missing in unit environment
            var tb = f.GetValue(page) as System.Web.UI.WebControls.TextBox;
            if (tb != null) tb.Text = value;
        }

        private static void SetLabel(object page, string fieldName)
        {
            var f = page.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null) return;
            if (f.GetValue(page) == null) f.SetValue(page, new System.Web.UI.WebControls.Label());
        }

        private static void SetPanel(object page, string fieldName)
        {
            var f = page.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null) return;
            if (f.GetValue(page) == null) f.SetValue(page, new System.Web.UI.WebControls.Panel());
        }

        private sealed class FakeDbProvider : IDbProvider
        {
            public string Name => "Fake";
            public bool TestConnection() => true;

            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "Q", "A" };
            public string GetPasswordByEmail(string email) => "pw";

            public System.Data.DataSet GetCatalogData() => throw new NotImplementedException();
            public bool RecreateGoatDb() => throw new NotImplementedException();
            public bool IsValidCustomerLogin(string email, string password) => throw new NotImplementedException();
            public string CustomCustomerLogin(string email, string password) => throw new NotImplementedException();
            public string GetCustomerEmail(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetCustomerDetails(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetOffice(string city) => throw new NotImplementedException();
            public System.Data.DataSet GetComments(string productCode) => throw new NotImplementedException();
            public string AddComment(string productCode, string email, string comment) => throw new NotImplementedException();
            public string UpdateCustomerPassword(int customerNumber, string password) => throw new NotImplementedException();
            public System.Data.DataSet GetUsers() => throw new NotImplementedException();
            public System.Data.DataSet GetOrders(int customerID) => throw new NotImplementedException();
            public System.Data.DataSet GetProductDetails(string productCode) => throw new NotImplementedException();
            public System.Data.DataSet GetOrderDetails(int orderNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetPayments(int customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetProductsAndCategories() => throw new NotImplementedException();
            public System.Data.DataSet GetProductsAndCategories(int catNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetEmailByName(string name) => throw new NotImplementedException();
            public string GetEmailByCustomerNumber(string num) => throw new NotImplementedException();
            public System.Data.DataSet GetCustomerEmails(string email) => throw new NotImplementedException();
        }
    }
}
