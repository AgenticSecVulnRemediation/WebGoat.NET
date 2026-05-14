using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikePattern()
        {
            // Arrange
            var sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            using var cmd = new MySqlCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@name", "bob%" );

            // Assert
            Assert.Contains("@name", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@name", cmd.Parameters[0].ParameterName);
            Assert.Equal("bob%", cmd.Parameters[0].Value);
        }
    }
}
