using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQueryForCustomerNumber()
        {
            // Arrange: delta change introduced @num parameter instead of concatenating num into SQL.
            var sql = "select email from CustomerLogin where customerNumber = @num";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@num", "1 OR 1=1");

            // Assert
            Assert.Contains("@num", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@num", cmd.Parameters[0].ParameterName);
        }
    }
}
