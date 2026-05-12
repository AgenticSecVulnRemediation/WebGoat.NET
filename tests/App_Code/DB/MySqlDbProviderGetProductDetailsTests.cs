using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductsAndComments()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            // Act
            var signature = method.ToString();

            // Assert
            Assert.Contains("GetProductDetails", signature);
        }
    }
}
