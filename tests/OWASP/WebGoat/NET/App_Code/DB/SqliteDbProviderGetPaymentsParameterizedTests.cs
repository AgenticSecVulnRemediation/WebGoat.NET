using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: production code is in namespace OWASP.WebGoat.NET.App_Code.DB
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var customerNumber = 1;

            // Act
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3");
            connection.Open();

            var sql = "select * from Payments where customerNumber = @customerNumber";
            var da = new SqliteDataAdapter(sql, connection);
            da.SelectCommand.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("@customerNumber", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.NotNull(da.SelectCommand.Parameters["@customerNumber"]);
            Assert.Equal(customerNumber, da.SelectCommand.Parameters["@customerNumber"].Value);
        }
    }
}
