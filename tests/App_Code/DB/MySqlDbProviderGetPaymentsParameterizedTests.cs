using Xunit;
using Moq;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesNamedParameter_DoesNotConcatenateCustomerNumber()
        {
            // Arrange
            var attackerSupplied = "1 OR 1=1";

            // Act
            var sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerSupplied, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = " + attackerSupplied, sql, StringComparison.Ordinal);
        }
    }
}
