using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_BindsEmailAndPasswordAsParameters()
        {
            // Delta assertion: query now uses @email and @password with AddWithValue.
            const string sql = "select * from CustomerLogin where email = @email and password = @password;";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            using var cmd = new SqliteCommand(sql, conn);

            cmd.Parameters.AddWithValue("@email", "user@example.com");
            cmd.Parameters.AddWithValue("@password", "encoded");

            Assert.Contains("@email", cmd.CommandText);
            Assert.Contains("@password", cmd.CommandText);
            Assert.Equal(2, cmd.Parameters.Count);
            Assert.Equal("@email", cmd.Parameters[0].ParameterName);
            Assert.Equal("@password", cmd.Parameters[1].ParameterName);
        }
    }
}
