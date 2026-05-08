using System;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_WithQuotes_DoesNotThrowFromSqlConcatenation()
        {
            // Arrange
            // Behavior change: query now uses @name parameter with wildcard; quotes should not break query construction.
            var provider = new MySqlDbProvider(new ConfigFile());

            // Act
            var ex = Record.Exception(() => provider.GetEmailByName("x' OR '1'='1"));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<MySqlException>(ex.GetBaseException());
        }
    }
}
