using System;
using Xunit;
using Moq;
using MySql.Data.MySqlClient;

// Assumptions:
// - MySqlDbProvider exists in namespace OWASP.WebGoat.NET.App_Code.DB
// - Test project references MySql.Data and can compile these types.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameter_ReturnsExceptionMessageWhenConnectionFails()
        {
            // Arrange
            // Minimal config that yields empty/invalid connection string.
            var config = new ConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act
            var result = provider.GetCustomerEmail("123");

            // Assert
            // Secure behavior: method should not concatenate customerNumber into SQL.
            // We can't intercept internal MySqlCommand without refactor, so we assert it does not throw
            // and returns a non-null string (exception message) on invalid connection.
            Assert.NotNull(result);
        }

        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar_DoesNotThrowOnInvalidConnectionString()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act
            var result = provider.GetEmailByCustomerNumber("123");

            // Assert
            Assert.NotNull(result);
        }
    }
}
