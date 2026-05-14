using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_AllowsEmailWithQuotes_WithoutThrowing_FromSqlConcatenation()
        {
            // Delta test for fix: SQL concatenation -> parameterized select.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            string email = "a' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail(email));

            // Assert
            Assert.Null(ex);
        }
    }
}
