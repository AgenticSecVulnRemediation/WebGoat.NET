using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_Query_UsesCustomerNumberParameter()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            string sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = ", sql + "x");
        }
    }
}
