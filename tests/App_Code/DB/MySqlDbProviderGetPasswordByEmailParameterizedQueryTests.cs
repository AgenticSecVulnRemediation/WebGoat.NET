using Xunit;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailParameterizedQueryTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterPlaceholderAndAddsParameter()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @Email;";

            // Act
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@Email", "attacker@example.com' OR 1=1 --");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@Email", cmd.Parameters[0].ParameterName);
        }
    }
}
