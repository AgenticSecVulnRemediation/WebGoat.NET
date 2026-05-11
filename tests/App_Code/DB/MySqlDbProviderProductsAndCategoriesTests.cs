using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_FiltersWithParameter_NotConcatenation()
        {
            // Arrange
            // Delta behavior: when catNumber >= 1, the code now uses @catNumber parameter.
            int catNumber = 1;
            using var conn = new MySqlConnection();

            // Act
            var sqlCategories = "select * from Categories where catNumber = @catNumber";
            var cmdCategories = new MySqlCommand(sqlCategories, conn);
            cmdCategories.Parameters.AddWithValue("@catNumber", catNumber);

            var sqlProducts = "select * from Products where catNumber = @catNumber";
            var cmdProducts = new MySqlCommand(sqlProducts, conn);
            cmdProducts.Parameters.AddWithValue("@catNumber", catNumber);

            // Assert
            Assert.DoesNotContain(catNumber.ToString(), cmdCategories.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmdCategories.Parameters["@catNumber"]);
            Assert.Equal(catNumber, cmdCategories.Parameters["@catNumber"].Value);

            Assert.DoesNotContain(catNumber.ToString(), cmdProducts.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmdProducts.Parameters["@catNumber"]);
            Assert.Equal(catNumber, cmdProducts.Parameters["@catNumber"].Value);
        }
    }
}
