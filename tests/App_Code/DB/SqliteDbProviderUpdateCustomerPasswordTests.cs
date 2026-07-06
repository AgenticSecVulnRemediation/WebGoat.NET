using System.IO;
using Xunit;

// Source-level regression test for PR 3965: UpdateCustomerPassword uses parameterized SQL.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var code = File.ReadAllText(path);

            Assert.Contains("UPDATE CustomerLogin SET password = @Password WHERE customerNumber = @CustomerNumber", code);
            Assert.Contains("Parameters.AddWithValue(\"@Password\"", code);
            Assert.Contains("Parameters.AddWithValue(\"@CustomerNumber\"", code);

            // Previously vulnerable concatenation
            Assert.DoesNotContain("update CustomerLogin set password = '\" +", code);
        }
    }
}
