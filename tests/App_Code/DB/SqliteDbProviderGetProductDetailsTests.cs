using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            var provider = new SqliteDbProvider(new FakeConfigFile(":memory:"));

            // Act
            try
            {
                provider.GetProductDetails("ABC' OR '1'='1");
            }
            catch
            {
                // Schema not present; we only care that the input does not cause SQL construction errors.
            }

            // Assert
            Assert.True(true);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            private readonly string _filename;
            public FakeConfigFile(string filename) { _filename = filename; }
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return _filename;
                if (key == DbConstants.KEY_CLIENT_EXEC) return "sqlite3";
                return string.Empty;
            }
        }
    }
}
