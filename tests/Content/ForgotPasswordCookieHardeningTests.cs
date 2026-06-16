using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Xunit;

// NOTE: This test uses reflection to invoke the handler and a minimal HttpContext.
// It focuses only on the delta behavior: the cookie must be HttpOnly.

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHardeningTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnlyTrue()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.ForgotPassword();

            // inject minimal required controls/fields via reflection
            SetPrivateField(page, "txtEmail", new System.Web.UI.WebControls.TextBox { Text = "user@example.com" });
            SetPrivateField(page, "labelQuestion", new System.Web.UI.WebControls.Label());
            SetPrivateField(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetPrivateField(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());

            // Replace DB provider with a stub
            SetPrivateField(page, "du", new StubDbProvider());

            var method = typeof(OWASP.WebGoat.NET.ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            method!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        private static void SetPrivateField(object target, string fieldOrPropertyName, object value)
        {
            var t = target.GetType();
            var field = t.GetField(fieldOrPropertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(target, value);
                return;
            }

            var prop = t.GetProperty(fieldOrPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
            {
                prop.SetValue(target, value, null);
                return;
            }

            throw new MissingMemberException(t.FullName, fieldOrPropertyName);
        }

        private class StubDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
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
            public string GetEmailByCustomerNumber(string num) => "user@example.com";
            public System.Data.DataSet GetCustomerEmails(string email) => new System.Data.DataSet();
        }
    }
}
