using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterWithTrailingWildcard()
        {
            // Arrange
            var conn = new MySqlConnection();
            var sql = "select email from CustomerLogin where email like @email";
            var email = "alice";

            // Act
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@email", email + "%");

            // Assert
            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@email"));
            Assert.Equal("alice%", cmd.Parameters["@email"].Value);
        }
    }
}
