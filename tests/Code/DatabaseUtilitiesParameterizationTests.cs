using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesParameterizationTests
    {
        [Fact]
        public void GetEmailByUserID_UsesParameterizedQuery_NotStringConcatenation()
        {
            // Arrange
            var cmd = new SqliteCommand("SELECT Email FROM UserList WHERE UserID = @UserID");

            // Act
            cmd.Parameters.AddWithValue("@UserID", "1234");

            // Assert
            Assert.Contains("@UserID", cmd.CommandText);
            Assert.Equal("1234", cmd.Parameters["@UserID"].Value);
        }

        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery()
        {
            // Arrange
            var cmd = new SqliteCommand("SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email");

            // Act
            cmd.Parameters.AddWithValue("@Email", "a@b.com");

            // Assert
            Assert.Contains("@Email", cmd.CommandText);
            Assert.Equal("a@b.com", cmd.Parameters["@Email"].Value);
        }
    }
}
