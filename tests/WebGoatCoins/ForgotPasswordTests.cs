using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumption: production project namespace is OWASP.WebGoat.NET.WebGoatCoins (from source file).
namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmail_Click_SetsSecurityAnswerCookieHttpOnlyTrue()
        {
            // Arrange
            var page = new ForgotPassword();

            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var response = new HttpResponse(new System.IO.StringWriter());
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Set up private field 'du' (IDbProvider) with a stub that returns a valid question/answer.
            // The project type for IDbProvider is inferred from usings in source.
            var stubProvider = new StubDbProvider();
            var field = typeof(ForgotPassword).GetField("du", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(page, stubProvider);

            // Also set the txtEmail Text property via reflection if it exists.
            SetTextBoxTextIfPresent(page, "txtEmail", "user@example.com");

            // Act
            var mi = typeof(ForgotPassword).GetMethod("ButtonCheckEmail_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi!.Invoke(page, new object[] { page, EventArgs.Empty });

            // Assert
            var cookie = HttpContext.Current.Response.Cookies["encr_sec_qu_ans"];
            Assert.NotNull(cookie);
            Assert.True(cookie!.HttpOnly);
        }

        private static void SetTextBoxTextIfPresent(object page, string fieldName, string value)
        {
            var f = page.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (f == null) return;
            var tb = f.GetValue(page);
            if (tb == null) return;

            var prop = tb.GetType().GetProperty("Text");
            if (prop == null || !prop.CanWrite) return;
            prop.SetValue(tb, value);
        }

        // Minimal stub to avoid external dependencies/mocking framework.
        // Only the method used by ButtonCheckEmail_Click is implemented.
        private sealed class StubDbProvider : OWASP.WebGoat.NET.App_Code.DB.IDbProvider
        {
            public string Name => "Stub";

            public bool TestConnection() => true;

            public string[] GetSecurityQuestionAndAnswer(string email)
                => new[] { "Q?", "A" };

            // The remaining interface members are not needed for this delta test.
            // Implemented to satisfy the interface at compile-time.
            public string GetPasswordByEmail(string email) => throw new NotImplementedException();
            public string GetCustomerNumberByEmail(string email) => throw new NotImplementedException();
            public bool GetCustomerExists(string email) => throw new NotImplementedException();
            public System.Data.DataSet GetCustomerByCustomerNumber(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetCustomersByName(string name) => throw new NotImplementedException();
            public System.Data.DataSet GetProductsByCategory(string category) => throw new NotImplementedException();
            public System.Data.DataSet GetProductsByCategoryAndName(string category, string name) => throw new NotImplementedException();
            public System.Data.DataSet GetProductByProductCode(string productCode) => throw new NotImplementedException();
            public System.Data.DataSet GetOrdersByCustomerNumber(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetPaymentsByCustomerNumber(string customerNumber) => throw new NotImplementedException();
            public System.Data.DataSet GetCommentsByProductCode(string productCode) => throw new NotImplementedException();
            public void AddComment(string productCode, string comment) => throw new NotImplementedException();
            public void RebuildDatabase() => throw new NotImplementedException();
            public bool UpdatePassword(string email, string password) => throw new NotImplementedException();
        }
    }
}
