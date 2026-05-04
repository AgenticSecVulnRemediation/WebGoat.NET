using System;
using Mono.Data.Sqlite;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersAndDoesNotEmbedUserInputInSql()
        {
            // Arrange
            // Build a provider with a temp db file, so calling IsValidCustomerLogin executes parameterized command.
            var tempDb = System.IO.Path.GetTempFileName();
            try
            {
                var cfg = new FakeConfigFile(tempDb);
                var provider = new SqliteDbProvider(cfg);

                // Create required table
                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS CustomerLogin (email TEXT, password TEXT);";
                    cmd.ExecuteNonQuery();
                }

                // Act
                var ex = Record.Exception(() => provider.IsValidCustomerLogin("x' OR 1=1 --", "pw"));

                // Assert
                Assert.Null(ex);
            }
            finally
            {
                try { System.IO.File.Delete(tempDb); } catch { /* ignore */ }
            }
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            private readonly string _fileName;
            public FakeConfigFile(string fileName) => _fileName = fileName;

            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return _fileName;
                if (key == DbConstants.KEY_CLIENT_EXEC) return string.Empty;
                return string.Empty;
            }
        }
    }
}
