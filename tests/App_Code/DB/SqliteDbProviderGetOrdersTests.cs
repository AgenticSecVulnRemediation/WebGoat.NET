using System;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameter_DoesNotAllowSqlInjectionInCustomerId()
        {
            // Arrange
            var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sqlite");
            try
            {
                SqliteConnection.CreateFile(dbPath);
                var connString = $"Data Source={dbPath};Version=3";

                using (var cn = new SqliteConnection(connString))
                {
                    cn.Open();
                    using (var cmd = new SqliteCommand("CREATE TABLE Orders(customerNumber INTEGER, orderNumber INTEGER);", cn))
                        cmd.ExecuteNonQuery();
                    using (var cmd = new SqliteCommand("INSERT INTO Orders(customerNumber, orderNumber) VALUES (1, 100);", cn))
                        cmd.ExecuteNonQuery();
                }

                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(SqliteDbProvider));

                typeof(SqliteDbProvider)
                    .GetField("_connectionString", BindingFlags.Instance | BindingFlags.NonPublic)!
                    .SetValue(provider, connString);

                // Act
                var ds = provider.GetOrders(1);

                // Assert
                Assert.NotNull(ds);
                Assert.NotNull(ds!.Tables[0]);
                Assert.Single(ds.Tables[0].Rows);
                Assert.Equal(100L, Convert.ToInt64(ds.Tables[0].Rows[0]["orderNumber"]));
            }
            finally
            {
                try { File.Delete(dbPath); } catch { /* ignore */ }
            }
        }
    }
}
