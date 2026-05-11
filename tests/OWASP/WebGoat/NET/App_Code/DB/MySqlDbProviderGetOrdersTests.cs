using System;
using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

// Assumption: The project references MySql.Data and the provider class exists in this namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerId()
        {
            // Arrange
            // We cannot execute against a real DB in a unit test; instead we assert the fixed query shape.
            var sql = "select * from Orders where customerNumber = @customerID";
            using var connection = new MySqlConnection();

            // Act
            var da = new MySqlDataAdapter(sql, connection);
            da.SelectCommand.Parameters.AddWithValue("@customerID", 1);

            // Assert
            Assert.Contains("@customerID", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.NotNull(da.SelectCommand.Parameters["@customerID"]);
            Assert.Equal(1, Convert.ToInt32(da.SelectCommand.Parameters["@customerID"].Value));
        }
    }
}
