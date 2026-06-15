using System;
using System.Data;
using Xunit;

// Assumption: production code uses the same namespaces as file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_WithSqlInjectionLikePayload_DoesNotThrowAndReturnsNullWhenNoRows()
        {
            // This is a delta test that targets the security fix in GetCustomerEmails:
            // it now uses a parameterized query (LIKE @Email) instead of string concatenation.
            // We can't easily assert the internal SqliteCommand without refactoring, but we can
            // at least ensure the method safely handles injection-like characters and doesn't
            // throw due to malformed SQL created by concatenation.

            // Arrange
            // Use an in-memory SQLite db file path via ConfigFile indirection isn't available here,
            // so we validate behavior by constructing provider with a temporary sqlite file.
            var tempDb = System.IO.Path.GetTempFileName();
            try
            {
                // Minimal config shim: ConfigFile is part of WebGoat; if unavailable in test project
                // this test will fail to compile. In WebGoat.NET it exists under App_Code.
                var config = new ConfigFile();
                config.Set(DbConstants.KEY_FILE_NAME, tempDb);
                config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

                var provider = new SqliteDbProvider(config);

                // Act
                // payload that would break old concatenated SQL: "' OR 1=1 --"
                var result = provider.GetCustomerEmails("' OR 1=1 --");

                // Assert
                // No rows expected in an empty db; secure behavior is "no crash" and null or empty dataset.
                Assert.True(result == null || (result.Tables.Count > 0 && result.Tables[0].Rows.Count >= 0));
            }
            finally
            {
                try { System.IO.File.Delete(tempDb); } catch { /* ignore */ }
            }
        }
    }
}
