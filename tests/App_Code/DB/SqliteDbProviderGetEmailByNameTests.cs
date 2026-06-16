using System;
using System.Data;
using System.IO;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_WithNameContainingQuote_TreatsAsLiteralAndReturnsNoRows()
        {
            // Arrange
            var dbFile = Path.Combine(Path.GetTempPath(), $"goatdb_{Guid.NewGuid():N}.sqlite");
            try
            {
                var cfg = new ConfigFile();
                cfg.Set(DbConstants.KEY_FILE_NAME, dbFile);
                cfg.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

                var provider = new SqliteDbProvider(cfg);

                using (var conn = new Mono.Data.Sqlite.SqliteConnection($"Data Source={dbFile};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Employees (firstName TEXT, lastName TEXT, email TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Employees(firstName, lastName, email) VALUES ('OReilly', 'Test', 'x@y.com');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act
                var ds = provider.GetEmailByName("O'Re");

                // Assert: should not throw; query should treat input as literal prefix and return no matches.
                Assert.NotNull(ds);
                Assert.Equal(0, ds.Tables[0].Rows.Count);
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }
    }
}
