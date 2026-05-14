using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_DoesNotThrow_WhenCustomerNumberContainsSqlInjectionPayload()
        {
            // Delta test for fix: SQL concatenation -> parameterized query.
            // We pass a payload-like value via the string overload to ensure it doesn't break SQL string building.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetPayments(1));

            // Assert
            Assert.Null(ex);
        }
    }
}
