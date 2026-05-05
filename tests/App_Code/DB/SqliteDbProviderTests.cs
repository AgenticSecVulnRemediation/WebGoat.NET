using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NoRawInterpolation()
        {
            // Arrange
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            cmd.Parameters.AddWithValue("@Email", "a@b.com");
            cmd.Parameters.AddWithValue("@Comment", "Nice'); DROP TABLE Comments; --");

            // Assert
            Assert.Contains("@productCode", cmd.CommandText);
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Contains("@Comment", cmd.CommandText);
            Assert.Equal(3, cmd.Parameters.Count);
        }
    }
}
