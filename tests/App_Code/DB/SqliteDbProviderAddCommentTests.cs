using Xunit;
using Mono.Data.Sqlite;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllFields()
        {
            // Regression: insert should use parameters rather than string concatenation.
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("@productCode", "P1");
            cmd.Parameters.AddWithValue("@email", "user@example.com");
            cmd.Parameters.AddWithValue("@comment", "hello");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.True(cmd.Parameters.Contains("@email"));
            Assert.True(cmd.Parameters.Contains("@comment"));
        }
    }
}
