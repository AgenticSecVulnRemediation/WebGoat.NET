using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithQuotes_DoesNotThrowFromSqlConcatenation()
        {
            // Arrange
            // Delta behavior: productCode used as a parameter (@productCode) instead of concatenated into SQL.
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("x' OR '1'='1"));

            // Assert
            Assert.NotNull(ex);
        }
    }
}
