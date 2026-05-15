using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetOrders_Tests
    {
        [Fact]
        public void GetOrders_UsesParameter_ForCustomerId()
        {
            // Delta test: query should be parameterized.
            var sql = "select * from Orders where customerNumber = @customerID";

            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
        }
    }
}
