using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryTemplate()
        {
            // This is a regression test for SQL injection fix: string concatenation -> parameters.
            // We validate by scanning the updated file for the parameterized SQL string.

            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("where email = @Email and password = @Password", content);
            Assert.DoesNotContain("where email = '\" + email", content);
        }
    }
}
