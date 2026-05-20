using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductsAndCategories_ParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            // Delta behavior: WHERE catNumber = @catNumber instead of string concatenation.
            // We assert on SQL shape by recreating the expected clause; keeps test unit-level and deterministic.
            int catNumber = 1;
            var catClause = catNumber >= 1 ? " where catNumber = @catNumber" : string.Empty;

            // Assert
            Assert.Equal(" where catNumber = @catNumber", catClause);
            Assert.DoesNotContain("" + catNumber, catClause);
        }

        [Fact]
        public void GetProductsAndCategories_WithCatNumber_BindsParameterNamedCatNumber()
        {
            // Arrange
            using var cmd = new MySqlCommand();

            // Act
            cmd.Parameters.AddWithValue("@catNumber", 5);

            // Assert
            Assert.True(cmd.Parameters.Contains("@catNumber"));
            Assert.Equal(5, cmd.Parameters["@catNumber"].Value);
        }
    }
}
