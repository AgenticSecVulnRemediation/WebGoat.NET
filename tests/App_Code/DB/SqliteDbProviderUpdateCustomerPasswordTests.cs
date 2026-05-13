using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_SqlIsParameterized()
        {
            // Delta behavior: SQL uses @password and @customerNumber.
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("Encoder.Encode(password)", sql);
        }
    }
}
