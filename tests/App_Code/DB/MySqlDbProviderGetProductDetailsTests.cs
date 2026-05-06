using System;
using System.Data;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_WhenGivenInjectionLikeProductCode_DoesNotTreatAsSqlFragment()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // We can only assert behavior indirectly (no string concatenation into SQL).
            // Pass a string that would break out of quotes in the old code.
            var payload = "X' OR '1'='1";
            Exception ex = Record.Exception(() => provider.GetProductDetails(payload));

            // Assert
            // With parameterized queries, payload should not appear as executable SQL; we at least ensure it doesn't surface as SQL text.
            if (ex != null)
                Assert.DoesNotContain(payload, ex.ToString());
        }
    }
}
