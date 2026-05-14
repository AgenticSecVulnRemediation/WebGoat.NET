using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_AllowsCustomerNumberWithSqlInjectionPayload_WithoutThrowing()
        {
            // Delta test for fix: concatenated query -> parameterized ExecuteScalar.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            string customerNumber = "1 OR 1=1";

            // Act
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber(customerNumber));

            // Assert
            Assert.Null(ex);
        }
    }
}
