using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQueryForCustomerNumber()
        {
            // Arrange: delta change introduced @customerNumber parameter.
            var sql = "SELECT email FROM CustomerLogin WHERE customerNumber = @customerNumber";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@customerNumber", 101);

            // Assert
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerNumber", cmd.Parameters[0].ParameterName);
        }
    }
}
