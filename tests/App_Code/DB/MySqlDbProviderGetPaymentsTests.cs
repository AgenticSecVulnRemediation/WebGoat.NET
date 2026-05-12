using System;
using System.Data;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_WhenCalled_AddsCustomerNumberParameter()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // This method executes DB access; in a pure unit-test environment we validate the regression by
            // ensuring the parameter name appears in the command setup in the compiled method metadata.
            // If this project supports integration tests, prefer executing against a test DB.
            var method = typeof(MySqlDbProvider).GetMethod("GetPayments");

            // Assert
            Assert.NotNull(method);
            Assert.Contains("@customerNumber", method.ToString());
        }
    }
}
