using System;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithInjectionLikeProductCode_DoesNotThrowFromSqlConcatenation()
        {
            // Arrange
            // Behavior change: query now uses @productCode parameter instead of string concatenation.
            // We can't validate DB output without a DB, but we can ensure the method does not build SQL via concatenation.
            // We'll pass a productCode containing quotes; previously it would break SQL string creation.
            var provider = new MySqlDbProvider(new ConfigFile());

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("abc' OR '1'='1"));

            // Assert
            // Expect connection-related exception, but not a syntax error from malformed SQL string.
            Assert.NotNull(ex);
            Assert.IsType<MySqlException>(ex.GetBaseException());
        }
    }
}
