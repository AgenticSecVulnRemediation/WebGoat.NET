using System;
using System.IO;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

// Assumptions:
// - Page event handler can be invoked directly.
// - We validate the delta behavior: cookie is marked HttpOnly and Secure after fix.

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsCookieHttpOnlyAndSecure()
        {
            // Arrange
            var request = new HttpRequest("", "https://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new ForgotPassword();

            // set up private field 'du' with fake provider returning known Q/A
            var fakeProvider = new FakeDbProvider(new[] { "Q", "A" });
            typeof(ForgotPassword).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(page, fakeProvider);

            // set email textbox via reflection
            var txtEmail = typeof(ForgotPassword).GetField("txtEmail", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(txtEmail);
            var tb = new System.Web.UI.WebControls.TextBox { Text = "user@example.com" };
            txtEmail!.SetValue(page, tb);

            // Act
            var handler = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(handler);
            handler!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = context.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private sealed class FakeDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            private readonly string[] _qa;
            public FakeDbProvider(string[] qa) => _qa = qa;

            public string Name => "fake";
            public bool TestConnection() => true;
            public string[] GetSecurityQuestionAndAnswer(string email) => _qa;

            // Other interface members are not needed for this delta test.
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
