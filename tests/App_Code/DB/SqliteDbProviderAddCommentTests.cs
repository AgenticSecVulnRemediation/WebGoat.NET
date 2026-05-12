using System;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesSqlParameters_ForAllValues()
        {
            // Arrange
            var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            try
            {
                SqliteConnection.CreateFile(dbPath);
                var cs = $"Data Source={dbPath};Version=3";

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Comments (productCode TEXT, email TEXT, comment TEXT);";
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();

                    var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        // Act
                        cmd.Parameters.AddWithValue("@productCode", "P1");
                        cmd.Parameters.AddWithValue("@email", "user@example.com");
                        cmd.Parameters.AddWithValue("@comment", "hello");
                        var affected = cmd.ExecuteNonQuery();

                        // Assert (delta): parameters exist and insert succeeds.
                        Assert.Equal(1, affected);
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@productCode");
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@email");
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@comment");
                    }

                    using (var verify = conn.CreateCommand())
                    {
                        verify.CommandText = "SELECT COUNT(*) FROM Comments;";
                        var count = Convert.ToInt64(verify.ExecuteScalar());
                        Assert.Equal(1L, count);
                    }
                }
            }
            finally
            {
                if (File.Exists(dbPath))
                    File.Delete(dbPath);
            }
        }
    }
}
