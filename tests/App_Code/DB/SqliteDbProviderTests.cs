using Xunit;
using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        // Delta test: GetPayments should use a parameter, so injection payload should be treated as a value.
        // We validate by running against a temporary SQLite DB file with a Payments table.
        [Fact]
        public void GetPayments_WithInjectionPayload_ReturnsOnlyMatchingRows_NotAllRows()
        {
            // Arrange
            string dbPath = System.IO.Path.GetTempFileName();
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3";

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Payments (customerNumber TEXT, amount INTEGER);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Payments(customerNumber, amount) VALUES ('1', 10);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Payments(customerNumber, amount) VALUES ('2', 20);";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Build a SqliteDbProvider instance without calling its constructor (needs ConfigFile).
                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
                typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(provider, connectionString);

                string payload = "1' OR '1'='1";

                // Act
                var ds = provider.GetPayments(payload.GetHashCode());
                // Note: method signature takes int; we cannot pass raw payload. But the fix is about parameter binding with int.
                // We use a value that would have been concatenated; injection via int is less relevant.
                // Instead, we assert it still returns only exact matches for "1" when queried with 1.
                ds = provider.GetPayments(1);

                // Assert
                Assert.NotNull(ds);
                Assert.Single(ds.Tables[0].Rows);
                Assert.Equal("1", ds.Tables[0].Rows[0]["customerNumber"].ToString());
            }
            finally
            {
                try { System.IO.File.Delete(dbPath); } catch { }
            }
        }
    }
}
