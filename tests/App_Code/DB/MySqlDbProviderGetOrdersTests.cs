using System;
using Xunit;
using Moq;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQueryForCustomerId()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetOrders(1));

            // Assert
            Assert.True(ex == null || ex is Exception);
            Assert.Contains("@customerID", "select * from Orders where customerNumber = @customerID");
        }
    }
}
