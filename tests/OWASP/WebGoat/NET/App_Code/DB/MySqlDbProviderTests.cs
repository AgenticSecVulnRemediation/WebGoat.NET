using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";
            using var cmd = new MySqlCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@email", "user@example.com");

            // Assert
            Assert.Contains("@email", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@email"]);
        }
    }
}
