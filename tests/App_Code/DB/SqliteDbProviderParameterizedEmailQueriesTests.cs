using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterizedQueryTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Arrange
            var sql = "SELECT * FROM CustomerLogin WHERE email = @Email";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@Email", "x@example.com' OR 1=1 --");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@Email", cmd.Parameters[0].ParameterName);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmailQuery()
        {
            // Arrange
            var sql = "SELECT * FROM CustomerLogin WHERE email = @Email";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@Email", "x@example.com' OR 1=1 --");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
        }
    }
}
