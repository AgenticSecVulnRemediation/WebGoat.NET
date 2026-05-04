using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Arrange
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            // Act
            // customerNumber is a string here; historically concatenated into SQL.
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("1 OR 1=1"));

            // Assert
            Assert.Null(ex);
            const string expectedSql = "select email from CustomerLogin where customerNumber = @CustomerNumber";
            Assert.Contains("@CustomerNumber", expectedSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
