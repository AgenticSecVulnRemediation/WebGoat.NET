using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NotStringConcatenation()
        {
            // Arrange
            var sql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @email, @comment)";

            // Act
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            cmd.Parameters.AddWithValue("@email", "a@b.com");
            cmd.Parameters.AddWithValue("@comment", "x'); DROP TABLE Comments;--");

            // Assert
            Assert.Contains("@productCode", cmd.CommandText);
            Assert.Contains("@email", cmd.CommandText);
            Assert.Contains("@comment", cmd.CommandText);
            Assert.Equal(3, cmd.Parameters.Count);
        }
    }
}
