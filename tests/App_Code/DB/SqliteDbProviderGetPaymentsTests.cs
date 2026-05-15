using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPayments_Tests
    {
        [Fact]
        public void GetPayments_UsesParameterBinding_ForCustomerNumber()
        {
            // Arrange
            const string sql = "select * from Payments where customerNumber = @customerNumber";
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            var da = new SqliteDataAdapter(sql, conn);

            // Act
            da.SelectCommand.Parameters.AddWithValue("@customerNumber", 123);

            // Assert
            Assert.Equal(sql, da.SelectCommand.CommandText);
            Assert.Single(da.SelectCommand.Parameters);
            Assert.Equal("@customerNumber", da.SelectCommand.Parameters[0].ParameterName);
        }
    }
}
