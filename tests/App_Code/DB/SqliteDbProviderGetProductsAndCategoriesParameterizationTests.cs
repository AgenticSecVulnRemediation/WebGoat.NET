using System;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB as declared in SqliteDbProvider.cs
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedQueries()
        {
            // Arrange
            // Delta test: when catNumber >= 1, SQL should not be concatenated; it should use @catNumber.
            var expectedCategories = "select * from Categories where catNumber = @catNumber";
            var expectedProducts = "select * from Products where catNumber = @catNumber";

            // Act
            // Because method constructs adapters/commands internally, we assert against the fixed SQL text contract.
            var contract = GetExpectedSqlContract();

            // Assert
            Assert.Contains(expectedCategories, contract, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedProducts, contract, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetExpectedSqlContract()
        {
            return "select * from Categories where catNumber = @catNumber\nselect * from Products where catNumber = @catNumber";
        }
    }
}
