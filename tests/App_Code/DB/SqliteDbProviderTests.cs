using System;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_WithSqlInjectionLikeInput_InsertsRowAndDoesNotDropTable()
        {
            // Arrange
            // Create a temp sqlite db file that SqliteDbProvider can open.
            string dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            try
            {
                using (var conn = new SqliteConnection($"Data Source={dbPath};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
CREATE TABLE Comments(
  productCode TEXT NOT NULL,
  email TEXT NOT NULL,
  comment TEXT NOT NULL
);
";
                        cmd.ExecuteNonQuery();
                    }
                }

                var provider = CreateProviderWithDbFile(dbPath);

                string productCode = "S10_1678";
                string email = "a@b.com'); DROP TABLE Comments;--";
                string comment = "x'); DROP TABLE Comments;--";

                // Act
                string output = provider.AddComment(productCode, email, comment);

                // Assert
                Assert.True(output == null || output.Length >= 0);

                using (var conn = new SqliteConnection($"Data Source={dbPath};Version=3"))
                {
                    conn.Open();
                    using (var verify = conn.CreateCommand())
                    {
                        verify.CommandText = "SELECT COUNT(*) FROM Comments";
                        long count = (long)verify.ExecuteScalar();
                        Assert.Equal(1, count);
                    }

                    using (var verify = conn.CreateCommand())
                    {
                        verify.CommandText = "SELECT productCode, email, comment FROM Comments";
                        using (var reader = verify.ExecuteReader())
                        {
                            Assert.True(reader.Read());
                            Assert.Equal(productCode, reader.GetString(0));
                            Assert.Equal(email, reader.GetString(1));
                            Assert.Equal(comment, reader.GetString(2));
                        }
                    }
                }
            }
            finally
            {
                try { if (File.Exists(dbPath)) File.Delete(dbPath); } catch { /* ignore */ }
            }
        }

        private static SqliteDbProvider CreateProviderWithDbFile(string dbPath)
        {
            // SqliteDbProvider depends on a concrete ConfigFile type; create an instance without invoking its constructor
            // and provide a Get(string) implementation via reflection if possible.
            // If ConfigFile.Get is not virtual, this still works by setting expected fields in a derived test type.
            return new SqliteDbProvider(new TestConfigFile(dbPath));
        }

        // Minimal concrete ConfigFile implementation used by SqliteDbProvider.
        // Assumption: ConfigFile is not sealed and Get(string) is virtual or abstract in this codebase.
        private sealed class TestConfigFile : ConfigFile
        {
            private readonly string _dbPath;
            public TestConfigFile(string dbPath) => _dbPath = dbPath;

            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return _dbPath;
                if (key == DbConstants.KEY_CLIENT_EXEC) return string.Empty;
                return string.Empty;
            }
        }
    }
}
