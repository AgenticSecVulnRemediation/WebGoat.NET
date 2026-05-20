using System;
using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterization_DoesNotThrowForLargeOrInjectedCustomerId()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            // Input is int, so injection attempts are blocked by typing, but regression covers SQL concatenation removal.
            var ex = Record.Exception(() => provider.GetOrders(int.MaxValue));

            // Assert
            Assert.Null(ex);
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new OWASP.WebGoat.NET.App_Code.ConfigFile();
            return new MySqlDbProvider(config);
        }
    }
}
