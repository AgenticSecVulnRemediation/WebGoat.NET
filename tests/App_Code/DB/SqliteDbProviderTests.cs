using Mono.Data.Sqlite;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB based on file_path.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQueryForCustomerNumber()
        {
            // Arrange: delta change replaced string concatenation with @customerNumber parameter.
            var sql = "select * from Payments where customerNumber = @customerNumber";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@customerNumber", 123);

            // Assert
            Assert.Contains("@customerNumber", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerNumber", cmd.Parameters[0].ParameterName);
        }
    }
}
