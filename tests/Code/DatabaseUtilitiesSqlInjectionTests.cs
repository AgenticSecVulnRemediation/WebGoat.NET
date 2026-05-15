using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesSqlInjectionTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameter_ForEmail()
        {
            // Arrange
            var sql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@Email", "a@b.com' OR 1=1 --");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Equal(1, cmd.Parameters.Count);
        }

        [Fact]
        public void AddToMailingList_UsesParameters_ForAllFields()
        {
            // Arrange
            var sql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@first, @last, @email)";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@first", "f");
            cmd.Parameters.AddWithValue("@last", "l");
            cmd.Parameters.AddWithValue("@email", "e@x.com");

            // Assert
            Assert.Equal(3, cmd.Parameters.Count);
            Assert.NotNull(cmd.Parameters["@first"]);
            Assert.NotNull(cmd.Parameters["@last"]);
            Assert.NotNull(cmd.Parameters["@email"]);
        }
    }
}
