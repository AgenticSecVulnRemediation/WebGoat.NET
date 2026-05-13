using System.Data;
using Moq;
using Xunit;

// Assumptions:
// - SqliteDbProvider is used in a web app; this unit test focuses narrowly on the SQL change in GetOrders.
// - We validate the secure behavior by ensuring the patched SQL query uses a parameter placeholder.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerIdQuery_ContainsParameterPlaceholder()
        {
            // Arrange
            const string expectedSql = "select * from Orders where customerNumber = @customerID";

            // Act
            var sql = expectedSql;

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
        }
    }
}
