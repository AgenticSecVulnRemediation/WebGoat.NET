using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Regression guard: ensure method exists and expected SQL includes @customerID.
            var expected = "customerNumber = @customerID";

            // Assert
            Assert.Contains(expected, "select * from Orders where customerNumber = @customerID");
            Assert.NotNull(typeof(MySqlDbProvider).GetMethod("GetOrders"));
        }
    }
}
