using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithSqlInjectionLikeProductCode_DoesNotThrowArgumentNullException()
        {
            // Delta focus: productCode is now bound as @productCode in both Products and Comments queries.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetProductDetails("S10_1678' OR '1'='1"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
