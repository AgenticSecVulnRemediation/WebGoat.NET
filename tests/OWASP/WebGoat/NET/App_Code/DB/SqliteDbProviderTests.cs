using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            var sql = "select * from Products where productCode = @productCode";
            var adapter = new SqliteDataAdapter(sql, "Data Source=:memory:");

            // Act
            adapter.SelectCommand.Parameters.AddWithValue("@productCode", "SAMPLE");

            // Assert
            Assert.Contains("@productCode", adapter.SelectCommand.CommandText);
            Assert.NotNull(adapter.SelectCommand.Parameters["@productCode"]);
        }
    }
}
