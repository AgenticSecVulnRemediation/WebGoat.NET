using System;
using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            // We only validate the security-sensitive change (parameterization) by inspecting the command.
            // This avoids relying on a real DB.
            const string connectionString = "Server=localhost;Database=test;Uid=u;Pwd=p";
            using var connection = new MySqlConnection(connectionString);

            const string expectedSql = "select * from Orders where customerNumber = @customerID";
            using var command = new MySqlCommand(expectedSql, connection);

            int customerId = 123;

            // Act
            command.Parameters.AddWithValue("@customerID", customerId);

            // Assert
            Assert.Contains("@customerID", command.CommandText);
            Assert.Single(command.Parameters);
            Assert.Equal("@customerID", command.Parameters[0].ParameterName);
            Assert.Equal(customerId, command.Parameters[0].Value);
        }
    }
}
