using System;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_PreventsSqlInjectionAndUpdatesRow()
        {
            // Arrange: create temp sqlite file and minimal schema/data
            var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sqlite");
            try
            {
                SqliteConnection.CreateFile(dbPath);
                var connString = $"Data Source={dbPath};Version=3";

                using (var cn = new SqliteConnection(connString))
                {
                    cn.Open();
                    using (var cmd = new SqliteCommand("CREATE TABLE CustomerLogin(customerNumber INTEGER PRIMARY KEY, password TEXT);", cn))
                        cmd.ExecuteNonQuery();
                    using (var cmd = new SqliteCommand("INSERT INTO CustomerLogin(customerNumber, password) VALUES (1, 'old');", cn))
                        cmd.ExecuteNonQuery();
                }

                // Create provider instance without calling its constructor (avoids ConfigFile dependency)
                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(SqliteDbProvider));

                typeof(SqliteDbProvider)
                    .GetField("_connectionString", BindingFlags.Instance | BindingFlags.NonPublic)!
                    .SetValue(provider, connString);

                // Act: attempt injection through password value; with parameterization it should not break SQL.
                var result = provider.UpdateCustomerPassword(1, "x', password='hacked");

                // Assert: no error reported and row updated exactly once
                Assert.True(result == null || result == string.Empty);

                using (var cn = new SqliteConnection(connString))
                {
                    cn.Open();
                    using var cmd = new SqliteCommand("SELECT password FROM CustomerLogin WHERE customerNumber=1;", cn);
                    var pwd = (string)cmd.ExecuteScalar();
                    Assert.NotEqual("old", pwd);
                    Assert.NotEqual("hacked", pwd);
                }
            }
            finally
            {
                try { File.Delete(dbPath); } catch { /* ignore */ }
            }
        }
    }
}
