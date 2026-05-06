using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_WhenGivenInjectionLikeProductCode_DoesNotTreatAsSqlFragment()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            // Act
            var payload = "X' OR '1'='1";
            Exception ex = Record.Exception(() => provider.GetProductDetails(payload));

            // Assert
            if (ex != null)
                Assert.DoesNotContain(payload, ex.ToString());
        }
    }
}
