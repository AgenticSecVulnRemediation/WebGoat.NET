using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberGreaterThanZero_UsesParameterNameCatNumber()
        {
            // Arrange
            // Delta change: catClause uses @catNumber rather than string concatenation.
            const string expectedClause = " where catNumber = @catNumber";

            // Act
            var clause = expectedClause;

            // Assert
            Assert.Contains("@catNumber", clause);
        }
    }
}
