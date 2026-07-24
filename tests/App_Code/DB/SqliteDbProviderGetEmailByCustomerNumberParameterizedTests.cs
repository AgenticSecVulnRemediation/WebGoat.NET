using Xunit;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_DoesNotConcatenateCustomerNumber()
        {
            // Arrange
            var attackerSupplied = "1 OR 1=1";

            // Act
            var sql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", sql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(attackerSupplied, sql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = " + attackerSupplied, sql, StringComparison.Ordinal);
        }
    }
}
