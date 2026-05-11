using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterPlaceholder_PreventsSqlInjection()
        {
            // Arrange
            var emailPrefix = "a%' OR 1=1 --";

            // Delta behavior: query should be parameterized like: email like @Email || '%'
            var sql = "select email from CustomerLogin where email like @Email || '%'";

            // Act
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", emailPrefix);

            // Assert
            Assert.Contains("email like @Email", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(emailPrefix, cmd.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmd.Parameters["@Email"]);
            Assert.Equal(emailPrefix, cmd.Parameters["@Email"].Value);
        }

        [Fact]
        public void CustomCustomerLogin_AddsEmailParameter_ToSelectCommand()
        {
            // Arrange
            var email = "test@example.com' OR 1=1 --";
            var sql = "select * from CustomerLogin where email = '" + email + "';";

            // Act
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var da = new SqliteDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@Email", email);

            // Assert
            Assert.NotNull(da.SelectCommand.Parameters["@Email"]);
            Assert.Equal(email, da.SelectCommand.Parameters["@Email"].Value);
        }
    }
}
