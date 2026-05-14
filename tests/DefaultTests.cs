using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using Xunit;

// Assumptions:
// - The project compiles System.Web and page lifecycle can be invoked via reflection.
// - We validate the delta behavior: newly created "Server" cookie is marked HttpOnly.

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_WhenDbConnectionSucceeds_SetsServerCookieHttpOnly()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new Default();

            // Inject a fake provider that returns true for TestConnection.
            var fakeProvider = new FakeDbProvider { TestConnectionResult = true, NameValue = "sqlite" };
            var field = typeof(Default).GetField("du", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(page, fakeProvider);

            // Act
            var pageLoad = typeof(Default).GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(pageLoad);
            pageLoad!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = context.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }

        private sealed class FakeDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public bool TestConnectionResult { get; set; }
            public string NameValue { get; set; } = "";

            public string Name => NameValue;

            public bool TestConnection() => TestConnectionResult;

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
            public string[] GetSecurityQuestionAndAnswer(string email) => throw new NotImplementedException();
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
