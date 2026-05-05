using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieTests
    {
        [Fact]
        public void Page_Load_WhenDbConfigured_AddsServerCookie_WithHttpOnlyAndSecure()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/Default.aspx", "");
            var responseWriter = new StringWriter();
            var response = new HttpResponse(responseWriter);
            var context = new HttpContext(request, response);

            HttpContext.Current = context;

            var page = new Default();

            // Inject a fake IDbProvider into the private field `du` so that the code path executes.
            // (This avoids external DB dependencies and makes the test deterministic.)
            var duField = typeof(Default).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new AlwaysTrueDbProvider());

            // Act
            var pageLoad = typeof(Default).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(pageLoad);
            pageLoad!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["Server"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        // Minimal stub matching the interface used by the page.
        private sealed class AlwaysTrueDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "Test";

            public bool TestConnection() => true;

            // Unused members for this test (throw to catch unexpected use)
            public void Dispose() { }
            public string GetCatNumberByProductCode(string productCode) => throw new NotImplementedException();
            public System.Data.DataSet GetCategoriesAndProducts(string catNumber) => throw new NotImplementedException();
            public string[] GetSecurityQuestionAndAnswer(string email) => throw new NotImplementedException();
            public string GetPasswordByEmail(string email) => throw new NotImplementedException();
            public void CreateUser(string username, string password, string email, string secQuestion, string secAnswer) => throw new NotImplementedException();
            public bool ValidateUser(string username, string password) => throw new NotImplementedException();
            public bool ChangePassword(string username, string oldPassword, string newPassword) => throw new NotImplementedException();
            public string[] GetUsers() => throw new NotImplementedException();
            public string[] GetRoles() => throw new NotImplementedException();
            public void AddUserToRole(string username, string roleName) => throw new NotImplementedException();
            public void RemoveUserFromRole(string username, string roleName) => throw new NotImplementedException();
            public bool IsUserInRole(string username, string roleName) => throw new NotImplementedException();
            public string[] GetRolesForUser(string username) => throw new NotImplementedException();
            public string[] GetUsersInRole(string roleName) => throw new NotImplementedException();
            public bool DeleteRole(string roleName) => throw new NotImplementedException();
            public bool DeleteUser(string username) => throw new NotImplementedException();
            public bool CreateRole(string roleName) => throw new NotImplementedException();
        }
    }
}
