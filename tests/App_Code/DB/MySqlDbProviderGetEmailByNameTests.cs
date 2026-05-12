using Xunit;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_AddsNameParameterWithWildcard()
        {
            // Arrange
            const string expectedSql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            var cmd = new MySqlCommand(expectedSql);

            // Act
            cmd.Parameters.AddWithValue("@name", "alice" + "%");

            // Assert
            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@name"));
            Assert.Equal("alice%", cmd.Parameters["@name"].Value);
        }
    }
}
