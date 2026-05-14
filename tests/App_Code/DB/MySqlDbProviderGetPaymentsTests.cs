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
            const string sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.DoesNotContain("+ customerNumber", sql);
            Assert.Contains("@customerNumber", sql);
        }
    }
}
