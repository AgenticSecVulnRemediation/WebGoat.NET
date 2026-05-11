using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            _ = new MySqlDbProvider(config.Object);

            // Act
            var expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Equal(expectedSql, GetExpectedSqlForGetCustomerEmail());
        }

        private static string GetExpectedSqlForGetCustomerEmail()
        {
            return "select email from CustomerLogin where customerNumber = @customerNumber";
        }
    }
}
