using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedScalarQuery()
        {
            // Regression test for SQL injection fix: ExecuteScalar uses parameter.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("where customerNumber = @customerNumber", content);
            Assert.Contains("new MySqlParameter(\"@customerNumber\"", content);
            Assert.DoesNotContain("where customerNumber = \" + num", content);
        }
    }
}
