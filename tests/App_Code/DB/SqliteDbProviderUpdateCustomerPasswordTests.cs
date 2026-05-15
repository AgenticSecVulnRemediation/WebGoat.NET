using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_AllowsSafeUpdate()
        {
            // Arrange
            var dbFile = ":memory:";
            var cs = "Data Source=:memory:;Version=3;New=True;";

            var config = new FakeConfigFile(new Dictionary<string, string>
            {
                { DbConstants.KEY_FILE_NAME, dbFile },
                { DbConstants.KEY_CLIENT_EXEC, string.Empty }
            });

            var provider = new SqliteDbProvider(config);
            typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(provider, cs);

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber INTEGER, password TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, password) VALUES (1, 'old');";
                cmd.ExecuteNonQuery();
            }

            // Act
            provider.UpdateCustomerPassword(1, "new'); DROP TABLE CustomerLogin; --");

            // Assert
            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "SELECT password FROM CustomerLogin WHERE customerNumber = 1";
                var pw = (string)cmd.ExecuteScalar();
                Assert.NotNull(pw);
            }
        }

        private class FakeConfigFile : ConfigFile
        {
            private readonly IDictionary<string, string> _values;
            public FakeConfigFile(IDictionary<string, string> values) => _values = values;
            public override string Get(string key) => _values.TryGetValue(key, out var v) ? v : string.Empty;
        }
    }
}
