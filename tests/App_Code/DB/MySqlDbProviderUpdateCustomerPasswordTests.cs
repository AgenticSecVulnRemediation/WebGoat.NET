using System;
using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_AllowsApostrophesInPassword_InputNotConcatenatedIntoSql()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(123, "pa'ss"));

            // Assert
            // Previously this could break SQL due to string concatenation.
            Assert.Null(ex);
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new OWASP.WebGoat.NET.App_Code.ConfigFile();
            return new MySqlDbProvider(config);
        }
    }
}
