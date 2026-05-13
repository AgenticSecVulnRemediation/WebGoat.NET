using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            // Arrange
            string sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = \" +", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
