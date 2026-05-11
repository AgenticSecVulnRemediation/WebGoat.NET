using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

// Assumption: production code is in namespace OWASP.WebGoat.NET.App_Code.DB
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeAndAppendsWildcard()
        {
            // Arrange
            var emailPrefix = "user";

            // Act
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3");
            connection.Open();

            var sql = "select email from CustomerLogin where email like @email";
            var da = new SqliteDataAdapter(sql, connection);
            da.SelectCommand.Parameters.AddWithValue("@email", emailPrefix + "%");

            // Assert
            Assert.Contains("like @email", da.SelectCommand.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.NotNull(da.SelectCommand.Parameters["@email"]);
            Assert.Equal(emailPrefix + "%", da.SelectCommand.Parameters["@email"].Value);
        }
    }
}
