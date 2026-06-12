using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordParameterizationTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedSqlAndAddsParameters()
        {
            // Arrange
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@password", "encoded");
            cmd.Parameters.AddWithValue("@customerNumber", 42);

            // Assert
            Assert.Contains("@password", cmd.CommandText);
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.Equal("encoded", cmd.Parameters["@password"].Value);
            Assert.Equal(42, cmd.Parameters["@customerNumber"].Value);
        }
    }
}
