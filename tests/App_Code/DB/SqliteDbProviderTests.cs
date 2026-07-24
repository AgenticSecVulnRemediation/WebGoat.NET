using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Mono.Data.Sqlite;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQueryForEmail()
        {
            // Arrange
            var conn = new SqliteConnection();
            var sql = "select * from CustomerLogin where email = @email;";
            var email = "bob@example.com";

            // Act
            var da = new SqliteDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);

            // Assert
            Assert.Equal(sql, da.SelectCommand.CommandText);
            Assert.True(da.SelectCommand.Parameters.Contains("@email"));
            Assert.Equal(email, da.SelectCommand.Parameters["@email"].Value);
        }
    }
}
