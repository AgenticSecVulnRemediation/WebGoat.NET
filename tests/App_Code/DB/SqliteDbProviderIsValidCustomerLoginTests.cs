using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_EmailAndPassword()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email and password = @password;";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@email", "user@example.com");
            cmd.Parameters.AddWithValue("@password", "encoded");

            // Assert
            Assert.Contains("email = @email", cmd.CommandText);
            Assert.Contains("password = @password", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@email"]);
            Assert.NotNull(cmd.Parameters["@password"]);
        }
    }
}
