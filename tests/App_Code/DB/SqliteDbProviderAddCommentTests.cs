using Xunit;
using Moq;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_WithMaliciousInput_DoesNotThrowDueToSqlSyntax()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(config.Object);

            // Override connection string to in-memory
            var connStrField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            connStrField!.SetValue(provider, "Data Source=:memory:;Version=3");

            using (var conn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE Comments(productCode TEXT, email TEXT, comment TEXT);";
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            var result = provider.AddComment("p", "e", "x'); DROP TABLE Comments;--");

            // Assert
            Assert.Null(result); // success returns null output
        }
    }
}
