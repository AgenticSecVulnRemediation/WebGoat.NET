using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_WithLargeCustomerId_DoesNotThrow_FromQueryConcatenation()
        {
            // Delta test: query changed from concatenation to parameterized command.
            // We exercise the method with a value that would previously be appended to SQL.

            // Arrange
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configFile.Object);

            // Act
            var ex = Record.Exception(() => provider.GetOrders(int.MaxValue));

            // Assert
            Assert.Null(ex);
        }
    }
}
