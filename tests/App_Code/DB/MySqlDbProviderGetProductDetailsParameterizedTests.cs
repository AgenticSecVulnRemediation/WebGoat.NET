using System;
using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_ProductCodeWithQuote_DoesNotThrowDueToSqlConcat()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("p'1"));

            // Assert
            Assert.Null(ex);
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new OWASP.WebGoat.NET.App_Code.ConfigFile();
            return new MySqlDbProvider(config);
        }
    }
}
