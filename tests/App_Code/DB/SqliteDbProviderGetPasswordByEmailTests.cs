using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPasswordByEmail_Tests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedSqliteCommand_InsteadOfStringConcatenation()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email;";
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = new SqliteCommand(sql, conn);

            // Act
            cmd.Parameters.AddWithValue("@email", "attacker@example.com' OR 1=1 --");
            var da = new SqliteDataAdapter(cmd);

            // Assert
            Assert.Equal(sql, da.SelectCommand.CommandText);
            Assert.Single(da.SelectCommand.Parameters);
            Assert.Equal("@email", da.SelectCommand.Parameters[0].ParameterName);
        }
    }
}
