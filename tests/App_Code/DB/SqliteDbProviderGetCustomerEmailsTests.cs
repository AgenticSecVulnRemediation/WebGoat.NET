using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Delta behavior: GetCustomerEmails uses parameterized LIKE with email + "%".

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLike_ReturnsOnlyPrefixMatches()
        {
            // Arrange
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE CustomerLogin(email TEXT);
INSERT INTO CustomerLogin(email) VALUES ('alice@example.com'), ('bob@example.com');
";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, cs);

            // Act
            var ds = provider.GetCustomerEmails("alice");

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables[0].Rows);
            Assert.Equal("alice@example.com", ds.Tables[0].Rows[0][0]);
        }
    }
}
