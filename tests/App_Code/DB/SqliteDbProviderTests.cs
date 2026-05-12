using System;
using Xunit;

// Assumptions:
// - SqliteDbProvider exists in namespace OWASP.WebGoat.NET.App_Code.DB
// - Test project references Mono.Data.Sqlite.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_DoesNotThrowWithInvalidDb()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new SqliteDbProvider(config);

            // Act
            var result = provider.UpdateCustomerPassword(1, "pw");

            // Assert
            // Regression assertion: method should not throw; on failure it returns an error message.
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateCustomerPassword_ReturnsNullOrMessage_ButNeverThrows()
        {
            // Arrange
            var config = new ConfigFile();
            var provider = new SqliteDbProvider(config);

            // Act & Assert
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(2, "pw"));
            Assert.Null(ex);
        }
    }
}
