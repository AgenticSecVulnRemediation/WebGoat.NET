using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithInjectedProductCode_DoesNotThrowSqlSyntaxErrors()
        {
            // Arrange
            // The fix changed GetProductDetails to use parameterized SqliteCommand (@productCode)
            // for both Products and Comments queries.
            var provider = CreateProviderWithInMemoryDb();
            var injected = "P1' OR 1=1 --";

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails(injected));

            // Assert
            Assert.Null(ex);
        }

        // Creates an instance with an in-memory sqlite DB file path in ConfigFile.
        private static SqliteDbProvider CreateProviderWithInMemoryDb()
        {
            var config = new FakeConfigFile();
            return new SqliteDbProvider(config);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                // Provide a file name to satisfy provider ctor.
                return key == DbConstants.KEY_FILE_NAME ? ":memory:" : "";
            }
        }
    }
}
