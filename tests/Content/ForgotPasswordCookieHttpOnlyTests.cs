using System;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookieHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            // Act
            // We can't run full WebForms lifecycle easily; instead, validate the fix by reflection:
            // the handler should set HttpCookie.HttpOnly = true on "encr_sec_qu_ans".
            // This test will fail if the property assignment is removed.
            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Presence of HttpOnly set is best asserted by executing with a fake HttpContext.
            // We'll do that: create HttpContext with HttpResponse and stub IDbProvider returning Q/A.

            var ctx = new System.Web.HttpContext(
                new System.Web.HttpRequest("", "http://localhost/ForgotPassword.aspx", ""),
                new System.Web.HttpResponse(new System.IO.StringWriter()));
            System.Web.HttpContext.Current = ctx;

            // Set required controls via reflection: txtEmail, panels, labelQuestion.
            SetControl(page, "txtEmail", new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            SetControl(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetControl(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());
            SetControl(page, "labelQuestion", new System.Web.UI.WebControls.Label());

            // Replace DB provider field 'du' with stub.
            var duField = typeof(ForgotPassword).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(duField);
            duField!.SetValue(page, new StubDbProvider());

            // invoke
            method.Invoke(page, new object[] { null, EventArgs.Empty });

            var cookie = ctx.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        private static void SetControl(object page, string fieldName, object control)
        {
            var f = page.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(f);
            f!.SetValue(page, control);
        }

        private class StubDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "stub";
            public bool TestConnection() => true;
            public System.Data.DataSet GetCatalogData() => new System.Data.DataSet();
            public bool IsValidCustomerLogin(string email, string password) => false;
            public bool RecreateGoatDb() => true;
            public string CustomCustomerLogin(string email, string password) => null;
            public string GetCustomerEmail(string customerNumber) => null;
            public System.Data.DataSet GetCustomerDetails(string customerNumber) => new System.Data.DataSet();
            public System.Data.DataSet GetOffice(string city) => new System.Data.DataSet();
            public System.Data.DataSet GetComments(string productCode) => new System.Data.DataSet();
            public string AddComment(string productCode, string email, string comment) => null;
            public string UpdateCustomerPassword(int customerNumber, string password) => null;
            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "Q", "A" };
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
