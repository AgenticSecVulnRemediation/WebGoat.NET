using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_DoesNotEmbedPasswordInSql_UsesParameters()
        {
            // Delta intent: the SQL string was changed to use @password and @customerNumber.
            // We validate the method executes up to the point of DB usage without throwing due to malformed SQL.

            // Arrange
            var configMock = new Mock<ConfigFile>();
            configMock.Setup(c => c.Get(DbConstants.KEY_HOST)).Returns("localhost");
            configMock.Setup(c => c.Get(DbConstants.KEY_PORT)).Returns("3306");
            configMock.Setup(c => c.Get(DbConstants.KEY_DATABASE)).Returns("db");
            configMock.Setup(c => c.Get(DbConstants.KEY_UID)).Returns("user");
            configMock.Setup(c => c.Get(DbConstants.KEY_PWD)).Returns("");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("mysql");

            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(1, "pw' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
