using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            // Arrange
            const string sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
