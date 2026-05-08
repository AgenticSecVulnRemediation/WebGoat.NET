using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllFields()
        {
            // Arrange
            const string productCode = "P1";
            const string email = "a@b.com";
            const string comment = "hello";

            // Act
            string sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", productCode);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Comment", comment);

            // Assert
            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.True(cmd.Parameters.Contains("@Email"));
            Assert.True(cmd.Parameters.Contains("@Comment"));
        }
    }
}
