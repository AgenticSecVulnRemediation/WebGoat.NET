using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_SqlText_UsesPasswordAndCustomerNumberParameters()
        {
            // Delta regression: PR changed SQL from concatenation to parameters.
            const string sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+", sql);
        }
    }
}
