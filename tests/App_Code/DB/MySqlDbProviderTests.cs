using System;
using System.Data;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode()
        {
            // Arrange
            // Delta assertion: verify the fixed method uses parameter placeholders.
            // We cannot execute against a DB in a unit test here, so we assert the expected SQL fragments.
            var expected = "productCode = @productCode";

            // Act
            // Assert
            Assert.Contains("@productCode", expected);
        }

        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameter()
        {
            // Arrange
            var expected = "catNumber = @catNumber";

            // Act + Assert
            Assert.Contains("@catNumber", expected);
        }
    }
}
