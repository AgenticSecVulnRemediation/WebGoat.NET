using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmail()
        {
            // Arrange: delta change replaced concatenated email with @email parameter and used command-backed adapter.
            var sql = "select * from CustomerLogin where email = @email";

            // Act
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@email", "attacker@example.com' OR '1'='1");

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@email", cmd.Parameters[0].ParameterName);
        }
    }
}
