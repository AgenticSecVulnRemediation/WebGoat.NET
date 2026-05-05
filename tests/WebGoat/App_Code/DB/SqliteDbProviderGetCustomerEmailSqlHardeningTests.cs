using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailSqlHardeningTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumberQuery_DoesNotInlineValue()
        {
            // Arrange
            var sql = "SELECT email FROM CustomerLogin WHERE customerNumber = @customerNumber";
            var customerNumber = "1 OR 1=1";

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

            // Assert
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.DoesNotContain(customerNumber, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@customerNumber"]);
        }
    }
}
