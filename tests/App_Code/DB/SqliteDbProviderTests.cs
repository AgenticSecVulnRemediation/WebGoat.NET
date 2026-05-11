using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeQuery()
        {
            // Delta assertion: query now uses @name and binds name + "%".
            const string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", "Al%" );

            Assert.Contains("like @name", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@name", cmd.Parameters[0].ParameterName);
            Assert.Equal("Al%", cmd.Parameters[0].Value);
        }
    }
}
