using System;
using System.Reflection;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPassword_HttpOnlyCookieTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsHttpOnlyOnSecurityAnswerCookie()
        {
            // Delta test for PR #637: cookie.HttpOnly should be set.

            var request = new System.Web.HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            var page = (ForgotPassword)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(ForgotPassword));

            SetPrivateField(page, "du", new FakeDbProvider());
            SetControlText(page, "txtEmail", "a@b.com");
            SetPrivateField(page, "labelQuestion", new System.Web.UI.WebControls.Label());
            SetPrivateField(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetPrivateField(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());

            InvokeHandler(page, "ButtonCheckEmail_Click");

            var cookie = System.Web.HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
        }

        private static void InvokeHandler(object page, string method)
        {
            var m = page.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(m);
            m!.Invoke(page, new object?[] { page, EventArgs.Empty });
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var f = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(target, value);
        }

        private static void SetControlText(object target, string fieldName, string text)
        {
            var tb = new System.Web.UI.WebControls.TextBox { Text = text };
            SetPrivateField(target, fieldName, tb);
        }

        private sealed class FakeDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "TEST";
            public bool TestConnection() => true;
            public System.Data.DataSet GetCatalogData() => throw new NotImplementedException();
            public bool IsValidCustomerLogin(string email, string password) => throw new NotImplementedException();
            public bool RecreateGoatDb() => throw new NotImplementedException();
            public string CustomCustomerLogin(string email, string password) => throw new NotImplementedException();
            public string GetCustomerEmail(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetCustomerDetails(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetOffice(string city) => throw new NotImplementedException();
            public System.Data.DataSet GetComments(string productCode) => throw new NotImplementedException();
            public string AddComment(string productCode, string email, string comment) => throw new NotImplementedException();
            public string UpdateCustomerPassword(int customerNumber, string password) => throw new NotImplementedException();
            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "Q", "A" };
            public string GetPasswordByEmail(string email) => throw new NotImplementedException();
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
