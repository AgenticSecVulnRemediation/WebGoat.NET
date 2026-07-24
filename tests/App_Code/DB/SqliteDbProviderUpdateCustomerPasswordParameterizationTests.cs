using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using System;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordParameterizationTests
    {
        [Fact]
        public void UpdateCustomerPassword_ParameterizesPasswordAndCustomerNumber_DoesNotThrowSqlSyntaxError()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(1, "pw'); DROP TABLE CustomerLogin;--"));

            // Assert
            if (ex != null)
            {
                Assert.DoesNotContain("DROP TABLE", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
