using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetPayments_Tests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999999)]
        public void GetPayments_WithVariousCustomerNumbers_DoesNotThrow(int customerNumber)
        {
            // Delta regression test: customerNumber is now bound via parameter (@customerNumber)
            // instead of concatenated into the SQL string.

            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "goatdb");
            config.Set(DbConstants.KEY_UID, "root");
            config.Set(DbConstants.KEY_PWD, "");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "mysql");

            var sut = new MySqlDbProvider(config);

            // Act
            var ex = Record.Exception(() => sut.GetPayments(customerNumber));

            // Assert
            Assert.Null(ex);
        }
    }
}
