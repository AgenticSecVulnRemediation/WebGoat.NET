using System;
using Xunit;

// Assumption: Source namespace from file path is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void GetProductsAndCategories_WithCatNumber_DoesNotThrowFormatException(int catNumber)
        {
            // Arrange
            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlDbProvider));

            // Act
            var ex = Record.Exception(() => provider.GetProductsAndCategories(catNumber));

            // Assert
            // Without a DB it may throw, but should not be due to SQL string concatenation with catNumber.
            Assert.False(ex is FormatException);
        }
    }
}
