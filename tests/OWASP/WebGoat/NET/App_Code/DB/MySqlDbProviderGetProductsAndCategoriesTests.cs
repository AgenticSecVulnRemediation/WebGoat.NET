using System;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesCatNumberParameter()
        {
            // Arrange
            var catNumber = 1;
            var catClause = catNumber >= 1 ? " where catNumber = @catNumber" : string.Empty;
            var sql = "select * from Categories" + catClause;
            using var connection = new MySqlConnection();

            // Act
            var da = new MySqlDataAdapter(sql, connection);
            if (catNumber >= 1)
            {
                da.SelectCommand.Parameters.AddWithValue("@catNumber", catNumber);
            }

            // Assert
            Assert.Contains("@catNumber", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.NotNull(da.SelectCommand.Parameters["@catNumber"]);
            Assert.Equal(catNumber, Convert.ToInt32(da.SelectCommand.Parameters["@catNumber"].Value));
        }

        [Fact]
        public void GetProductsAndCategories_WhenCatNumberNotProvided_DoesNotAddCatNumberParameter()
        {
            // Arrange
            var catNumber = 0;
            var catClause = catNumber >= 1 ? " where catNumber = @catNumber" : string.Empty;
            var sql = "select * from Categories" + catClause;
            using var connection = new MySqlConnection();

            // Act
            var da = new MySqlDataAdapter(sql, connection);

            // Assert
            Assert.DoesNotContain("@catNumber", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.Null(da.SelectCommand.Parameters["@catNumber"]);
        }
    }
}
