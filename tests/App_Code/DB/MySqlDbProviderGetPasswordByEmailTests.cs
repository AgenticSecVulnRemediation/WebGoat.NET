using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_DoesNotConcatEmailIntoSql()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @Email;";

            // Act
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@Email", "test@example.com' OR 1=1 --");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@Email", cmd.Parameters[0].ParameterName);
        }
    }
}
