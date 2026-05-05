using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_BindsNameParameter_WithTrailingWildcard()
        {
            // Arrange
            const string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            const string connectionString = "Server=localhost;Database=test;Uid=u;Pwd=p";
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand(sql, connection);

            // Act
            command.Parameters.AddWithValue("@name", "Alice" + "%");

            // Assert
            Assert.Contains("like @name", command.CommandText);
            Assert.Single(command.Parameters);
            Assert.Equal("@name", command.Parameters[0].ParameterName);
            Assert.Equal("Alice%", command.Parameters[0].Value);
        }
    }
}
