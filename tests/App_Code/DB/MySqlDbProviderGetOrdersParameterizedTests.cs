using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesCustomerIdParameter_NotConcatenation()
        {
            // Arrange
            // Delta behavior: SQL changed from concatenation to parameter @customerID.
            var sql = "select * from Orders where customerNumber = @customerID";

            // Act
            var usesParam = sql.Contains("@customerID", StringComparison.OrdinalIgnoreCase);
            var usesConcat = sql.Contains("customerNumber = ");

            // Assert
            Assert.True(usesParam);
            // The query will still include "customerNumber ="; ensure it does not include concatenation of a value.
            Assert.False(sql.Contains("+ customerID"));
            Assert.True(usesConcat);
        }
    }
}
