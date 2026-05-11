using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithQuoteInProductCode_DoesNotThrowFromSqlConcatenation()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("ABC'XYZ"));

            // Assert
            Assert.Null(ex);
        }
    }
}
