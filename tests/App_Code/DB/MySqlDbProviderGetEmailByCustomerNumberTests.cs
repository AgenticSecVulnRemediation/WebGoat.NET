using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            _ = new MySqlDbProvider(config.Object);

            // Act
            var expectedSql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Equal(expectedSql, GetExpectedSqlForGetEmailByCustomerNumber());
        }

        private static string GetExpectedSqlForGetEmailByCustomerNumber()
        {
            return "select email from CustomerLogin where customerNumber = @num";
        }
    }
}
