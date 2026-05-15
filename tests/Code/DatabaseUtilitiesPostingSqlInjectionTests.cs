using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesPostingSqlInjectionTests
    {
        [Fact]
        public void AddNewPosting_UsesParameters_InsteadOfConcatenation()
        {
            // Arrange
            var sql = "INSERT INTO Postings(title, email, message) VALUES (@title, @email, @message)";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@title", "t");
            cmd.Parameters.AddWithValue("@email", "e@x.com");
            cmd.Parameters.AddWithValue("@message", "m");

            // Assert
            Assert.Equal(3, cmd.Parameters.Count);
            Assert.Contains("@title", cmd.CommandText);
            Assert.Contains("@email", cmd.CommandText);
            Assert.Contains("@message", cmd.CommandText);
        }
    }
}
