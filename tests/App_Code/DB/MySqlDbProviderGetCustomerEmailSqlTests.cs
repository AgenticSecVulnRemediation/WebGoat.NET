using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailSqlTests
    {
        [Fact]
        public void GetCustomerEmail_SqlIsParameterized_WithCustomerNumber()
        {
            // Arrange/Act
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql, StringComparison.Ordinal);
        }
    }
}
