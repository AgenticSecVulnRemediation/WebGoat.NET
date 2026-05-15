using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerId()
        {
            // Arrange
            // We can't run against a real MySQL server; assert that the query string was changed to use @customerID.
            // This delta test prevents regression back to string concatenation (SQL injection).
            var source = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Act & Assert
            Assert.Contains("select * from Orders where customerNumber = @customerID", source);
            Assert.Contains("Parameters.AddWithValue(\"@customerID\"", source);
            Assert.DoesNotContain("customerNumber = \" + customerID", source);
        }
    }
}
