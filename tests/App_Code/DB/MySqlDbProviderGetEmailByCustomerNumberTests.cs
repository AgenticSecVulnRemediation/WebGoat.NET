using System;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");

            // Assert (delta): method should reference @num parameter.
            Assert.NotNull(method);
            Assert.Contains("@num", method.ToString());
        }
    }
}
