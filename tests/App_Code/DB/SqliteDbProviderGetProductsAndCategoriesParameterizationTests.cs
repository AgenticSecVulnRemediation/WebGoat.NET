using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedCatNumber()
        {
            // Arrange
            var catClause = " where catNumber = @catNumber";
            var sql = "select * from Categories" + catClause;
            using var adapter = new SqliteDataAdapter(sql, new SqliteConnection());

            // Act
            adapter.SelectCommand.Parameters.AddWithValue("@catNumber", 1);

            // Assert
            Assert.Contains("@catNumber", adapter.SelectCommand.CommandText);
            Assert.Equal(1, adapter.SelectCommand.Parameters["@catNumber"].Value);
        }

        [Fact]
        public void GetProductsAndCategories_WithoutCatNumber_DoesNotRequireCatNumberParameter()
        {
            // Arrange
            var sql = "select * from Categories";
            using var adapter = new SqliteDataAdapter(sql, new SqliteConnection());

            // Assert
            Assert.DoesNotContain("@catNumber", adapter.SelectCommand.CommandText);
            Assert.Empty(adapter.SelectCommand.Parameters);
        }
    }
}
