using System;
using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class WebGoatCoinsForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookieHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            var ctx = new System.Web.HttpContext(
                new System.Web.HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", ""),
                new System.Web.HttpResponse(new System.IO.StringWriter()));
            System.Web.HttpContext.Current = ctx;

            SetControl(page, "txtEmail", new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            SetControl(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetControl(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());
            SetControl(page, "labelQuestion", new System.Web.UI.WebControls.Label());

            var duField = typeof(ForgotPassword).GetField("du", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(duField);
            duField!.SetValue(page, new StubDbProvider());

            var method = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            method!.Invoke(page, new object[] { null, EventArgs.Empty });

            // Assert
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
