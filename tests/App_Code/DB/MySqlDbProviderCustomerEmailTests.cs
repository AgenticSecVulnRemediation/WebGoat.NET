using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_ForCustomerNumber()
        {
            // Arrange
            var customerNumber = "1 OR 1=1";
            using var conn = new MySqlConnection();

            // Act
            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("customerNumber = @customerNumber", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(customerNumber, cmd.CommandText, StringComparison.Ordinal);
            Assert.Equal(customerNumber, cmd.Parameters["@customerNumber"].Value);
        }
    }
}
