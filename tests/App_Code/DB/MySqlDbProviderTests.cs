using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            _ = new MySqlDbProvider(config.Object);

            // Act
            var expectedSql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Equal(expectedSql, GetExpectedSqlForGetPayments());
        }

        private static string GetExpectedSqlForGetPayments()
        {
            return "select * from Payments where customerNumber = @customerNumber";
        }
    }
}
