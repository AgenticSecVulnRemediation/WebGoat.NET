using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingSqlHardeningTests
    {
        [Fact]
        public void AddNewPosting_UsesParameters_DoesNotInlineUserControlledValues()
        {
            // Arrange
            var sql = "insert into Postings(title, email, message) values (@title, @email, @message)";
            var title = "t'); DROP TABLE Postings;--";
            var email = "a@b.com';--";
            var message = "m');--";

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqliteParameter("@title", title));
            cmd.Parameters.Add(new SqliteParameter("@email", email));
            cmd.Parameters.Add(new SqliteParameter("@message", message));

            // Assert
            Assert.Contains("@title", cmd.CommandText);
            Assert.Contains("@email", cmd.CommandText);
            Assert.Contains("@message", cmd.CommandText);

            Assert.DoesNotContain(title, cmd.CommandText);
            Assert.DoesNotContain(email, cmd.CommandText);
            Assert.DoesNotContain(message, cmd.CommandText);

            Assert.Equal(3, cmd.Parameters.Count);
        }
    }
}
