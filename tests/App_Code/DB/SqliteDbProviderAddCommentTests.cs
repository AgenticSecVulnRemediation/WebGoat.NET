using System;
using System.Reflection;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_AllowsSqlMetaCharsInCommentAndDoesNotModifySchema()
        {
            // Arrange
            // Delta test: AddComment changed to parameterized insert.

            var dbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wg_sqlite_comment_" + Guid.NewGuid() + ".db");
            var cs = $"Data Source={dbPath};Version=3";

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE Comments (productCode TEXT, email TEXT, comment TEXT);";
                    cmd.ExecuteNonQuery();
                }
            }

            var cfg = new Moq.Mock<ConfigFile>(Moq.MockBehavior.Loose);
            cfg.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns(dbPath);
            cfg.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("");
            var provider = new SqliteDbProvider(cfg.Object);

            // Act
            var result = provider.AddComment("p1", "e@example.com", "hi'); DROP TABLE Comments;--");

            // Assert
            Assert.True(string.IsNullOrEmpty(result), "Expected no error output for parameterized insert.");

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Comments";
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    Assert.Equal(1, count);
                }
            }

            try { System.IO.File.Delete(dbPath); } catch { /* ignore */ }
        }
    }
}
