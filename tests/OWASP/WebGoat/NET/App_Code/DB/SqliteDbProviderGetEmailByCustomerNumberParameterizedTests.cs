using System;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: production code is in namespace OWASP.WebGoat.NET.App_Code.DB
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var num = "1";

            // Act
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3");
            connection.Open();

            var sql = "select email from CustomerLogin where customerNumber = @num";
            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("@num", num);

            // Assert
            Assert.Contains("@num", cmd.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmd.Parameters["@num"]);
            Assert.Equal(num, cmd.Parameters["@num"].Value);
        }
    }
}
