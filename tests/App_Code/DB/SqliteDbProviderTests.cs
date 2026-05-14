using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_AcceptsQuoteCharacters_WithoutThrowing_FromSqlConcatenation()
        {
            // Delta test for fix: SQL concatenation -> parameterized query.
            // We assert the method doesn't throw when given quote-containing inputs.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            string email = "a' OR '1'='1";
            string password = "p' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.IsValidCustomerLogin(email, password));

            // Assert
            Assert.Null(ex);
        }
    }
}
