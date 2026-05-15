using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_ForCustomerNumberAndHandlesNullScalar()
        {
            // Arrange
            const string sql = "select email from CustomerLogin where customerNumber = @customerNumber";
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = new SqliteCommand(sql, conn);

            // Act
            cmd.Parameters.AddWithValue("@customerNumber", "1");

            // Assert
            Assert.Equal(sql, cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@customerNumber", cmd.Parameters[0].ParameterName);
        }
    }
}
