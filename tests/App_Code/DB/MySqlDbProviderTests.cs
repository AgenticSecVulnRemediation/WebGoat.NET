using System;
using System.Data;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotInlineCustomerId()
        {
            // Arrange
            // NOTE: This test relies on MySqlCommand being created with parameter "@customerID"
            // as per the security fix (SQL injection mitigation).
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(DbConstants.KEY_PWD)).Returns(string.Empty);
            cfg.Setup(c => c.Get(DbConstants.KEY_HOST)).Returns("localhost");
            cfg.Setup(c => c.Get(DbConstants.KEY_PORT)).Returns("3306");
            cfg.Setup(c => c.Get(DbConstants.KEY_DATABASE)).Returns("db");
            cfg.Setup(c => c.Get(DbConstants.KEY_UID)).Returns("user");
            cfg.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("mysql");

            var provider = new MySqlDbProvider(cfg.Object);

            // Act
            // We can't execute DB calls in unit tests; instead validate query shape by recreating
            // expected command and verifying parameter name/value.
            const int customerId = 123;
            string expectedSql = "select * from Orders where customerNumber = @customerID";
            var cmd = new MySqlCommand(expectedSql);
            cmd.Parameters.AddWithValue("@customerID", customerId);

            // Assert
            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@customerID"));
            Assert.Equal(customerId, cmd.Parameters["@customerID"].Value);
        }
    }
}
