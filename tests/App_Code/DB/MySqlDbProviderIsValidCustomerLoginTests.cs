using System;
using System.Collections.Specialized;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithSqlInjectionInEmail_DoesNotThrowFromConcatenatedSql()
        {
            // Arrange
            var nvc = new NameValueCollection
            {
                [DbConstants.KEY_HOST] = "localhost",
                [DbConstants.KEY_PORT] = "3306",
                [DbConstants.KEY_DATABASE] = "db",
                [DbConstants.KEY_UID] = "u",
                [DbConstants.KEY_PWD] = "",
                [DbConstants.KEY_CLIENT_EXEC] = "mysql"
            };
            var provider = new MySqlDbProvider(new ConfigFile(nvc));

            // Act
            var ex = Record.Exception(() => provider.IsValidCustomerLogin("x' OR '1'='1", "pass"));

            // Assert
            Assert.Null(ex);
        }
    }
}
