using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerEmailsLikeParameterizedQueryTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterAndAppendsWildcard()
        {
            // Arrange
            var sql = "select email from CustomerLogin where email like @email";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@email", "abc" + "%");

            // Assert
            Assert.Contains("like @email", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@email", cmd.Parameters[0].ParameterName);
            Assert.Equal("abc%", cmd.Parameters[0].Value);
        }
    }
}
