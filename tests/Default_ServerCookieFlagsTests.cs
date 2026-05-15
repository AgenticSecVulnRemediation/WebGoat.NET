using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class Default_ServerCookieFlagsTests
    {
        [Fact]
        public void PageLoad_WhenDbConfigured_AddsServerCookieWithHttpOnlyAndSecure()
        {
            // Delta test for PR #634: "Server" cookie should be HttpOnly and Secure.

            var request = new System.Web.HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext.Current = new System.Web.HttpContext(request, response);

            // Create page instance
            var page = new Default();

            // Inject a fake DB provider that returns true for TestConnection
            var duField = typeof(Default).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new AlwaysOkDbProvider());

            // Act
            InvokePageLoad(page);

            // Assert
            var cookie = System.Web.HttpContext.Current.Response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void InvokePageLoad(Default page)
        {
            var m = typeof(Default).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(m);
            m!.Invoke(page, new object?[] { page, EventArgs.Empty });
        }

        private sealed class AlwaysOkDbProvider : IDbProvider
        {
            public string Name => "TEST";
            public bool TestConnection() => true;

            // The rest of interface members are not used by Default.Page_Load
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
