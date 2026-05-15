using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesCustomerIdParameter_InsteadOfConcatenation()
        {
            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";

            // Act/Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
        }
    }
}
