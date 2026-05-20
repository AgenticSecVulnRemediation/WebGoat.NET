using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumption: Default page is in namespace OWASP.WebGoat.NET and class name Default.
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieTests
    {
        [Fact]
        public void PageLoad_WhenDbConnectionSucceeds_SetsServerCookieHttpOnly()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new Default();

            // Replace the DB provider with a fake that returns true for TestConnection
            var duField = typeof(Default).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new FakeDbProvider());

            // Provide lblOutput used by Page_Load
            var lblField = typeof(Default).GetField("lblOutput", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(lblField);
            lblField!.SetValue(page, new System.Web.UI.WebControls.Label());

            // Act
            page.GetType().GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(page, new object?[] { null, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
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
