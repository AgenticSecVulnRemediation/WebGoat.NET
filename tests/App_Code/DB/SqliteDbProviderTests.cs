using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetPayments_DoesNotThrow_WhenCustomerNumberProvided()
        {
            // Delta test for fix: Payments query now uses parameter binding.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetPayments(1));

            // Assert
            Assert.Null(ex);
        }
    }
}
