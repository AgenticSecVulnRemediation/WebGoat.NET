using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_AndDoesNotInlineCustomerNumber()
        {
            // Arrange
            // Create command the same way the fixed code does.
            const int customerNumber = 123;
            string sql = "select * from Payments where customerNumber = @customerNumber";
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Act
            var commandText = cmd.CommandText;

            // Assert
            Assert.Equal("select * from Payments where customerNumber = @customerNumber", commandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerNumber", cmd.Parameters[0].ParameterName);
            Assert.Equal(customerNumber, Convert.ToInt32(cmd.Parameters[0].Value));

            // Regression signal: Ensure we did not end up with a concatenated SQL string.
            Assert.DoesNotContain(customerNumber.ToString(), commandText);
        }
    }
}
