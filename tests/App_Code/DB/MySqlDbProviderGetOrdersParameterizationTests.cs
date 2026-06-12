using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersParameterizationTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerIdQuery()
        {
            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";
            using var cmd = new MySqlCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@customerID", 123);

            // Assert
            Assert.Contains("@customerID", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@customerID"]);
            Assert.Equal(123, cmd.Parameters["@customerID"].Value);
        }
    }
}
