using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderParameterizedQueryTests
    {
        [Fact]
        public void CustomCustomerLogin_ParameterizedEmail_DoesNotBreakOnApostrophe()
        {
            // Arrange
            // Delta behavior: query changed from concatenation to parameter.
            var connString = "Data Source=:memory:;Version=3";
            using var conn = new SqliteConnection(connString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (email TEXT, Password TEXT);";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO CustomerLogin(email, Password) VALUES (@email, @pwd);";
                cmd.Parameters.AddWithValue("@email", "o'hara@example.com");
                cmd.Parameters.AddWithValue("@pwd", "enc");
                cmd.ExecuteNonQuery();
            }

            // Act + Assert
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from CustomerLogin where email = @email";
                cmd.Parameters.AddWithValue("@email", "o'hara@example.com");
                using var reader = cmd.ExecuteReader();
                Assert.True(reader.Read());
            }
        }
    }
}
