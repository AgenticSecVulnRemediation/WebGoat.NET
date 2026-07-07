using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_WithInjectionPayload_DoesNotThrow()
        {
            // Arrange
            // Delta: SQL now uses LIKE @email, with parameter value email + "%".
            var provider = new SqliteDbProvider(new FakeConfigFile());
            var injected = "a%' OR 1=1 --";

            // Act
            var ex = Record.Exception(() => provider.GetCustomerEmails(injected));

            // Assert
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
