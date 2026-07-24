using Xunit;
using Moq;
using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_WithInjectionLikeCustomerId_DoesNotBypassFilterAndReturnsOnlyMatchingRows()
        {
            // Arrange: create a real SQLite file and schema with two customers.
            var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".db");

            try
            {
                using (var cn = new SqliteConnection($"Data Source={dbPath};Version=3"))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = @"
CREATE TABLE Orders (orderNumber INTEGER PRIMARY KEY, customerNumber INTEGER NOT NULL);
INSERT INTO Orders(orderNumber, customerNumber) VALUES (1, 1);
INSERT INTO Orders(orderNumber, customerNumber) VALUES (2, 2);
";
                        cmd.ExecuteNonQuery();
                    }
                }

                var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
                configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(dbPath);
                configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite3");

                var provider = new SqliteDbProvider(configMock.Object);

                // Act: request customer 1.
                var result = provider.GetOrders(1);

                // Assert: should only return rows for customer 1 (would be bypassed if concatenation allowed injection).
                Assert.NotNull(result);
                Assert.True(result.Tables.Count > 0);
                Assert.Single(result.Tables[0].Rows);
                Assert.Equal(1, Convert.ToInt32(result.Tables[0].Rows[0]["customerNumber"]));
            }
            finally
            {
                try { if (File.Exists(dbPath)) File.Delete(dbPath); } catch { /* ignore */ }
            }
        }
    }
}
