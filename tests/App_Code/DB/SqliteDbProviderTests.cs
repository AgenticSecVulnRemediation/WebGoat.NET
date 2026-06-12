using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        private sealed class DummyConfigFile : ConfigFile
        {
            private readonly string _dbPath;
            public DummyConfigFile(string dbPath) { _dbPath = dbPath; }
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return _dbPath;
                if (key == DbConstants.KEY_CLIENT_EXEC) return string.Empty;
                return string.Empty;
            }
        }

        [Fact]
        public void GetPayments_WithInjectionLikeCustomerNumber_DoesNotBypassFilterAndReturnsNull()
        {
            // Arrange: create temp sqlite file and minimal schema
            string dbFile = System.IO.Path.GetTempFileName();
            try
            {
                using (var conn = new SqliteConnection($"Data Source={dbFile};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Payments (customerNumber TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Payments(customerNumber) VALUES ('1');";
                        cmd.ExecuteNonQuery();
                    }
                }

                var provider = new SqliteDbProvider(new DummyConfigFile(dbFile));

                // This would have selected all rows if concatenated: customerNumber = 0 OR 1=1
                string injected = "0 OR 1=1";

                // Act
                DataSet result = provider.GetPayments(injected);

                // Assert: parameterized query should not match, so provider returns null
                Assert.Null(result);
            }
            finally
            {
                try { System.IO.File.Delete(dbFile); } catch { /* ignore */ }
            }
        }
    }
}
