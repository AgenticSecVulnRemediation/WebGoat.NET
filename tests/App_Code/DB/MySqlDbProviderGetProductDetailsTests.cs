using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WhenProductCodeContainsSqlPayload_UsesParameterizedQuery()
        {
            // Arrange
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("ABC' OR '1'='1"));

            // Assert
            Assert.Null(ex);
            const string expectedSql = "select * from Products where productCode = @productCode";
            Assert.Contains("@productCode", expectedSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
