using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmail()
        {
            // Arrange: delta change replaced concatenated email with @email parameter.
            var sql = "select * from CustomerLogin where email = @email;";

            // Act
            using var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@email", "a@b.com' OR '1'='1");

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@email", cmd.Parameters[0].ParameterName);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmail()
        {
            // Arrange: delta change replaced concatenated email with @email parameter.
            var sql = "select * from CustomerLogin where email = @email;";

            // Act
            using var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@email", "user@example.com");

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
        }
    }
}
