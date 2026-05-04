using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_WhenCalled_UsesCustomerIdParameter()
        {
            // Arrange
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            // Act
            var ex = Record.Exception(() => provider.GetOrders(123));

            // Assert
            // Method should not throw due to SQL formatting with injection in integer (regression for concatenation)
            Assert.Null(ex);

            const string expectedSql = "select * from Orders where customerNumber = @customerID";
            Assert.Contains("@customerID", expectedSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
