using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Regression test for SQL injection fix: customerNumber concatenation removed.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("where customerNumber = @customerNumber", content);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", content);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", content);
        }
    }
}
