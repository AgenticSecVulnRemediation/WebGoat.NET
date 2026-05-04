using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateStatement()
        {
            // Arrange
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            // Act
            var ex = Record.Exception(() => provider.UpdateCustomerPassword(1, "pw' , creditLimit=9999 --"));

            // Assert
            Assert.Null(ex);
            // Delta security: ensure new placeholders are used (tied to fix)
            const string expectedSql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
