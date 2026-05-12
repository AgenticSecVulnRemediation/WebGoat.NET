using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_SqlString_UsesParameters()
        {
            // Arrange/Act
            string source = typeof(MySqlDbProvider).ToString();

            // Assert (delta)
            Assert.Contains("SET password = @password", source);
            Assert.Contains("WHERE customerNumber = @customerNumber", source);
        }
    }
}
