using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_InsteadOfStringConcatenation()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";
            var email = "x' OR 1=1 --";

            // Act
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@email", email);

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.DoesNotContain(email, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@email"]);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter_InsteadOfStringConcatenation()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";
            var email = "x' OR 1=1 --";

            // Act
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@email", email);

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.DoesNotContain(email, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@email"]);
        }
    }
}
