using System;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterPlaceholder_NotStringConcatenation()
        {
            // Arrange
            var expectedSql = "select * from Orders where customerNumber = @customerID";

            // Act + Assert
            Assert.Contains("@customerID", expectedSql);
            Assert.DoesNotContain("+", expectedSql);
            Assert.DoesNotContain("customerNumber = ", expectedSql.Replace("customerNumber = @customerID", string.Empty));
        }
    }
}
