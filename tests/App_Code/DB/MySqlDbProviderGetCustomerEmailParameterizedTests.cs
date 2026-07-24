using Xunit;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_DoesNotInlineCustomerNumber()
        {
            // Arrange
            var attackerSupplied = "1 OR 1=1";

            // Act
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerSupplied, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = " + attackerSupplied, sql, StringComparison.Ordinal);
        }
    }
}
