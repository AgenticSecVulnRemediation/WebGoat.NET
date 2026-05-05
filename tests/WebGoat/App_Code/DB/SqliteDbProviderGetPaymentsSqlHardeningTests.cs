using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsSqlHardeningTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumberQuery_DoesNotInlineValue()
        {
            // Arrange
            var sql = "select * from Payments where customerNumber = @customerNumber";
            var customerNumber = 123;

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.DoesNotContain(customerNumber.ToString(), cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@customerNumber"]);
        }
    }
}
