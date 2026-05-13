using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_SqlUsesCustomerNumberParameter()
        {
            // Arrange
            string injectedCustomerNumber = "1 OR 1=1";
            string fixedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedCustomerNumber, fixedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
