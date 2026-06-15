using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery()
        {
            // Regression test for SQL injection fix: update statement now uses parameters.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("update CustomerLogin set password = @password where customerNumber = @customerNumber", content);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", content);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", content);
            Assert.DoesNotContain("set password = '\" + Encoder.Encode(password)", content);
        }
    }
}
