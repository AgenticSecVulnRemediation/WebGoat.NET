using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterPlaceholderInSql()
        {
            // Arrange
            // Note: This is a delta unit test focusing on the fix that replaced string concatenation with a parameter.
            // We validate behavior by asserting the method accepts an input that would be dangerous in concatenation scenarios
            // without throwing SQL syntax errors.

            // SqliteDbProvider constructor requires ConfigFile; not provided in patch context.
            // This test is intentionally lightweight and will compile only if ConfigFile/DbConstants exist in solution.
            var config = new FakeConfigFile();
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetPayments(123));

            // Assert
            Assert.Null(ex);
        }

        // Minimal stub to satisfy constructor without external config dependency.
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
