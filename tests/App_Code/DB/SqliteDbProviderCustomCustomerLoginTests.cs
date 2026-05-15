using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery_DoesNotBreakOnQuoteInEmail()
        {
            // Arrange
            // Security regression: previously string concatenation could break query or enable injection.
            // Here we ensure the updated code can accept email with a quote without throwing due to SQL syntax.
            // We'll run against an in-memory sqlite db with a minimal CustomerLogin table.

            var dbFile = ":memory:";
            var cs = "Data Source=:memory:;Version=3;New=True;";

            // Create provider and inject connection string via reflection
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
                cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT, Password TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin(Email, Password) VALUES ('test@example.com', 'pw');";
                cmd.ExecuteNonQuery();
            }

            // Act
            var result = provider.CustomCustomerLogin("bad'email@example.com", "pw");

            // Assert
            // With parameterized query, this should not throw; it should return Email Address Not Found!
            Assert.Equal("Email Address Not Found!", result);
        }

        // Minimal in-test ConfigFile implementation (assumes ConfigFile is not sealed and has virtual Get)
        private class FakeConfigFile : ConfigFile
        {
            private readonly IDictionary<string, string> _values;
            public FakeConfigFile(IDictionary<string, string> values) => _values = values;
            public override string Get(string key) => _values.TryGetValue(key, out var v) ? v : string.Empty;
        }
    }
}
