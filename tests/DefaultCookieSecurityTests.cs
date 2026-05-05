using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using OWASP.WebGoat.NET;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSecurityTests
    {
        [Fact]
        public void Page_Load_WhenDbConfigured_SetsServerCookieHttpOnlyAndSecure()
        {
            // Delta test: cookie.HttpOnly and cookie.Secure were added.

            // Arrange
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new Default();

            // Replace the IDbProvider field with a stub that returns true for TestConnection
            var duField = typeof(Default).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new DbProviderStub());

            // Act
            var method = typeof(Default).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            method!.Invoke(page, new object[] { null!, EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private sealed class DbProviderStub : IDbProvider
        {
            public string Name => "stub";

            public bool TestConnection() => true;

            // The rest of IDbProvider members are not used by this test.
            public string[] GetSecurityQuestionAndAnswer(string email) => throw new NotImplementedException();
            public string GetPasswordByEmail(string email) => throw new NotImplementedException();
            public bool CustomerLogin(string username, string password) => throw new NotImplementedException();
            public string GetCustomerName(string email) => throw new NotImplementedException();
            public string GetCustomerEmail(string customerNumber) => throw new NotImplementedException();
            public string GetCustomerNumber(string email) => throw new NotImplementedException();
            public System.Data.DataSet GetOrdersByCustomerNumber(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetProductsAndCommentsByCatNumber(string catNumber) => throw new NotImplementedException();
            public System.Data.DataSet SearchProductsByName(string name) => throw new NotImplementedException();
            public System.Data.DataSet GetAllProductsAndComments() => throw new NotImplementedException();
            public void AddComment(string email, string comment) => throw new NotImplementedException();
            public System.Data.DataSet GetPayments(string email) => throw new NotImplementedException();
            public void UpdatePassword(string email, string password) => throw new NotImplementedException();
        }
    }
}
