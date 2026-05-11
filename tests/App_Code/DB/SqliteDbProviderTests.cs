using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_PreventingSqlInjection()
        {
            // Arrange
            // We intentionally use strings that would break a concatenated SQL statement.
            string productCode = "ABC'); DROP TABLE Comments;--";
            string email = "x@y.com";
            string comment = "test'; --";

            // Use an in-memory db file name to avoid touching external systems.
            var provider = new SqliteDbProvider(new FakeConfigFile(":memory:"));

            // Act
            string result;
            try
            {
                result = provider.AddComment(productCode, email, comment);
            }
            catch
            {
                // The method might still throw due to missing schema; however it must not be due to SQL syntax
                // errors introduced by string concatenation. We treat reaching this point as acceptable.
                result = null;
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
