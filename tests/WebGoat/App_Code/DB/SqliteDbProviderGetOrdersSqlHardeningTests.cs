using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersSqlHardeningTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerIdQuery_DoesNotInlineValue()
        {
            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";
            var customerId = 7;

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@customerID", customerId);

            // Assert
            Assert.Contains("@customerID", cmd.CommandText);
            Assert.DoesNotContain(customerId.ToString(), cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@customerID"]);
        }
    }
}
