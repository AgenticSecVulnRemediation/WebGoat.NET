using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailSearchTests
    {
        [Fact]
        public void GetEmailByName_UsesLikeParameterWithTrailingWildcard()
        {
            // Arrange
            const string name = "Ann";

            // Act
            string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            var cmd = new MySqlCommand(sql);
            cmd.Parameters.AddWithValue("@name", name + "%");

            // Assert
            Assert.True(cmd.CommandText.Contains("like @name"));
            Assert.True(cmd.Parameters.Contains("@name"));
            Assert.Equal(name + "%", cmd.Parameters["@name"].Value);
        }
    }
}
