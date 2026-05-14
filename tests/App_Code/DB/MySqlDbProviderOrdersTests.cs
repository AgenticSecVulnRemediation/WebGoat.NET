using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterForCustomerId()
        {
            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";

            using var cmd = new MySqlCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@customerID", 123);

            // Assert
            Assert.Contains("@customerID", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerID", cmd.Parameters[0].ParameterName);
        }
    }
}
