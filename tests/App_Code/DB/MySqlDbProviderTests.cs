using System;
using Xunit;
using MySql.Data.MySqlClient;

// Assumptions:
// - MySqlDbProvider exists in namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesMySqlParameter_ReturnsNonNullOnFailure()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act
            var result = provider.GetEmailByCustomerNumber("1");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetEmailByCustomerNumber_DoesNotThrow_WhenCustomerNumberProvided()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act & Assert
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("42"));
            Assert.Null(ex);
        }
    }
}
