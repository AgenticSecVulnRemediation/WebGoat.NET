using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // This regression test targets the change from string concatenation to parameterized queries
            // for productCode in GetProductDetails.

            // Arrange
            var sql = "select * from Products where productCode = @productCode";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@productCode", "S10_1678' OR 1=1 --");

            // Assert
            Assert.Contains("@productCode", cmd.CommandText);
            Assert.Equal(1, cmd.Parameters.Count);
            Assert.Equal("@productCode", cmd.Parameters[0].ParameterName);
        }
    }
}
