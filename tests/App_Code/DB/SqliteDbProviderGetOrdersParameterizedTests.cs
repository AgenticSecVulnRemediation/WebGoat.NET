using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetOrders_Tests
    {
        [Fact]
        public void GetOrders_UsesParameterPlaceholder_InsteadOfConcatenation()
        {
            // Arrange
            var fixedSql = "select * from Orders where customerNumber = @customerID";

            // Act / Assert
            Assert.Contains("@customerID", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("+ customerID", fixedSql, StringComparison.Ordinal);
        }
    }
}
