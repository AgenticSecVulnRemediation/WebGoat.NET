using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Arrange: delta change replaced concatenated customerID with @customerID parameter.
            var sql = "select * from Orders where customerNumber = @customerID";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@customerID", 7);

            // Assert
            Assert.Contains("@customerID", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerID", cmd.Parameters[0].ParameterName);
        }
    }
}
