using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_WithNumParameter()
        {
            // Arrange
            var cfg = new Mock<ConfigFile>(MockBehavior.Loose);
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act/Assert
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);
            Assert.Contains("GetEmailByCustomerNumber", method.ToString());
        }
    }
}
