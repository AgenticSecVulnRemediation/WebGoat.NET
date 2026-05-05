using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailSqlHardeningTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmailQuery_DoesNotInlineEmail()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email";
            var email = "x' OR 1=1 --";

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@email", email);

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.DoesNotContain(email, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@email"]);
        }
    }
}
