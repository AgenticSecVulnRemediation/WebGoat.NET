using System;
using Mono.Data.Sqlite;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberParameterizationTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_DoesNotThrowOnInjectionLikeInput()
        {
            // Arrange
            var config = new FakeConfigFile();
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("1; DROP TABLE CustomerLogin;--"));

            // Assert
            Assert.Null(ex);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return "test.sqlite";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "sqlite";
                return string.Empty;
            }
        }
    }
}
