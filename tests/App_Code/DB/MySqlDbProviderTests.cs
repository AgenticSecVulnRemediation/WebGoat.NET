using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_AllowsPasswordWithQuotes_WithoutThrowing_FromSqlConcatenation()
        {
            // Delta test for fix: SQL concatenation -> parameterized update.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            int customerNumber = 1;
            string password = "pw' ); DROP TABLE CustomerLogin;--";

            // Act
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(customerNumber, password));

            // Assert
            Assert.Null(ex);
        }
    }
}
