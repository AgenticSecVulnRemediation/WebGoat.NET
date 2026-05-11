using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_WithSqlMetaCharacters_DoesNotBreakInsertStatement()
        {
            // Arrange
            // Build a temporary sqlite database file and minimal schema so the provider can execute the insert.
            var tempDb = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            SqliteConnection.CreateFile(tempDb);

            var connString = $"Data Source={tempDb};Version=3";
            using (var conn = new SqliteConnection(connString))
            {
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "CREATE TABLE Comments(productCode TEXT, email TEXT, comment TEXT);";
                cmd.ExecuteNonQuery();
            }

            // Create provider instance without ConfigFile by using reflection to set private fields.
            // (We only need AddComment behavior changed in diff.)
            var provider = (SqliteDbProvider)Activator.CreateInstance(typeof(SqliteDbProvider), nonPublic: true)!;
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(provider, connString);

            var comment = "nice'); DROP TABLE Comments; --";

            // Act
            var result = provider.AddComment("S10_1678", "a@b.com", comment);

            // Assert
            Assert.True(string.IsNullOrEmpty(result)); // output is null on success

            using (var conn = new SqliteConnection(connString))
            {
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Comments;";
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                Assert.Equal(1, count);

                cmd.CommandText = "SELECT comment FROM Comments LIMIT 1;";
                var saved = (string)cmd.ExecuteScalar();
                Assert.Equal(comment, saved);
            }
        }
    }
}
