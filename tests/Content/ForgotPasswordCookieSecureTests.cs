using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: ForgotPassword code-behind is in namespace OWASP.WebGoat.NET and class name ForgotPassword.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordSecureCookieTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookieSecureAndHttpOnly()
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new ForgotPassword();

            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new FakeDbProvider());

            SetPrivateControlText(page, "txtEmail", "user@example.com");
            SetPrivateControl(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetPrivateControl(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());
            SetPrivateControl(page, "labelQuestion", new System.Web.UI.WebControls.Label());

            // Act
            page.GetType().GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void SetPrivateControlText(object page, string fieldName, string text)
        {
            var tb = new System.Web.UI.WebControls.TextBox { Text = text };
            SetPrivateControl(page, fieldName, tb);
        }

        private static void SetPrivateControl(object page, string fieldName, Control control)
        {
            var field = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(page, control);
        }

        private sealed class FakeDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "fake";
            public bool TestConnection() => true;
            public System.Data.DataSet GetCatalogData() => new System.Data.DataSet();
            public bool RecreateGoatDb() => true;
            public bool IsValidCustomerLogin(string email, string password) => true;
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
