using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET as declared in ForgotPassword.aspx.cs
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookie_AsHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            // Wire up minimal HttpContext with response cookies
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            // Replace private field 'du' with a mock implementation via reflection
            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new StubDbProvider());

            // Create required controls (expected by event handlers)
            var txtEmail = new TextBox { Text = "user@example.com" };
            var labelQuestion = new Label();
            var panel2 = new Panel();
            var panel3 = new Panel();

            typeof(ForgotPassword).GetField("txtEmail", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(page, txtEmail);
            typeof(ForgotPassword).GetField("labelQuestion", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(page, labelQuestion);
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep2", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(page, panel2);
            typeof(ForgotPassword).GetField("PanelForgotPasswordStep3", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(page, panel3);

            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            // Act
            method!.Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
        }

        private sealed class StubDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "stub";
            public bool TestConnection() => true;
            public System.Data.DataSet GetCatalogData() => new System.Data.DataSet();
            public bool RecreateGoatDb() => true;
            public bool IsValidCustomerLogin(string email, string password) => true;
            public string CustomCustomerLogin(string email, string password) => null;
            public string GetCustomerEmail(string customerNumber) => "user@example.com";
            public System.Data.DataSet GetCustomerDetails(string customerNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetOffice(string city) => new System.Data.DataSet();
            public System.Data.DataSet GetComments(string productCode) => new System.Data.DataSet();
            public string AddComment(string productCode, string email, string comment) => null;
            public string UpdateCustomerPassword(int customerNumber, string password) => null;
            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "question?", "answer" };
            public string GetPasswordByEmail(string email) => "pw";
            public System.Data.DataSet GetUsers() => new System.Data.DataSet();
            public System.Data.DataSet GetOrders(int customerID) => new System.Data.DataSet();
            public System.Data.DataSet GetProductDetails(string productCode) => new System.Data.DataSet();
            public System.Data.DataSet GetOrderDetails(int orderNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetPayments(int customerNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetProductsAndCategories() => new System.Data.DataSet();
            public System.Data.DataSet GetProductsAndCategories(int catNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetEmailByName(string name) => new System.Data.DataSet();
            public string GetEmailByCustomerNumber(string num) => "user@example.com";
            public System.Data.DataSet GetCustomerEmails(string email) => new System.Data.DataSet();
        }
    }
}
