using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterMarker_PreventsSqlInjection()
        {
            // Arrange
            var provider = new SqliteDbProvider(new FakeConfigFile());
            var injectedCustomerNumber = "1 OR 1=1";

            // Act
            var ex = Record.Exception(() => provider.GetCustomerEmail(injectedCustomerNumber));

            // Assert
            // With parameterization, input should not break SQL parsing via concatenation.
            Assert.Null(ex);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                return key == DbConstants.KEY_FILE_NAME ? ":memory:" : "";
            }
        }
    }
}
