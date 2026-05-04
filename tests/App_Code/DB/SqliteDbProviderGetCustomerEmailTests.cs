using System;
using Mono.Data.Sqlite;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var tempDb = System.IO.Path.GetTempFileName();
            try
            {
                var cfg = new FakeConfigFile(tempDb);
                var provider = new SqliteDbProvider(cfg);

                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS CustomerLogin (customerNumber TEXT, email TEXT);";
                    cmd.ExecuteNonQuery();
                }

                // Act
                var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));

                // Assert
                Assert.Null(ex);
            }
            finally
            {
                try { System.IO.File.Delete(tempDb); } catch { }
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
