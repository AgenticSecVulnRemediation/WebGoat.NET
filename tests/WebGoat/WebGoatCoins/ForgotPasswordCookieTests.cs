using System;
using System.IO;
using System.Reflection;
using System.Web;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_AddsSecurityAnswerCookie_WithHttpOnlyAndSecure()
        {
            // Arrange
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            var page = new ForgotPassword();

            // Inject fake DB provider so GetSecurityQuestionAndAnswer returns an existing user.
            var duField = typeof(ForgotPassword).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(duField);
            duField!.SetValue(page, new FakeDbProvider());

            // Provide a fake txtEmail control with email text.
            var txtEmailField = typeof(ForgotPassword).GetField("txtEmail", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(txtEmailField);
            var emailTextBox = new System.Web.UI.WebControls.TextBox { Text = "user@example.com" };
            txtEmailField!.SetValue(page, emailTextBox);

            // Provide required label/panel controls accessed by method.
            SetControl(page, "labelQuestion", new System.Web.UI.WebControls.Label());
            SetControl(page, "PanelForgotPasswordStep2", new System.Web.UI.WebControls.Panel());
            SetControl(page, "PanelForgotPasswordStep3", new System.Web.UI.WebControls.Panel());

            // Act
            var handler = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(handler);
            handler!.Invoke(page, new object?[] { page, EventArgs.Empty });

            // Assert
            var cookie = response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
            Assert.True(cookie.Secure);
        }

        private static void SetControl(object page, string fieldName, object control)
        {
            var f = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(f);
            f!.SetValue(page, control);
        }

        private sealed class FakeDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "Test";
            public bool TestConnection() => true;
            public string[] GetSecurityQuestionAndAnswer(string email) => new[] { "q", "a" };

            public void Dispose() { }
            public string GetCatNumberByProductCode(string productCode) => throw new NotImplementedException();
            public System.Data.DataSet GetCategoriesAndProducts(string catNumber) => throw new NotImplementedException();
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
