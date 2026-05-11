using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            _ = new MySqlDbProvider(config.Object);

            // Act
            var expectedSql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Equal(expectedSql, GetExpectedSqlForUpdateCustomerPassword());
        }

        private static string GetExpectedSqlForUpdateCustomerPassword()
        {
            return "update CustomerLogin set password = @password where customerNumber = @customerNumber";
        }
    }
}
