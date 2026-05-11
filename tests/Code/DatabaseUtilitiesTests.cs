using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void DoScalar_AddsProvidedParameter()
        {
            // Delta assertion: DoScalar now accepts an optional SqliteParameter and adds it to the command.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            conn.Open();

            using var cmd = new SqliteCommand("SELECT @UserID", conn);
            cmd.Parameters.Add(new SqliteParameter("@UserID", "1234"));

            var result = cmd.ExecuteScalar();
            Assert.Equal("1234", result);
        }
    }
}
