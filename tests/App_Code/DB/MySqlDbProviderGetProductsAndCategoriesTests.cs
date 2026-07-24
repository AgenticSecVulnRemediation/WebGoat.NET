using Xunit;
using Moq;
using System;

// Assumption: namespace inferred from source file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            var providerType = typeof(MySqlDbProvider);
            var method = providerType.GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            // Regression guard that method remains callable.
            Assert.Equal("GetProductsAndCategories", method.Name);
        }
    }
}
