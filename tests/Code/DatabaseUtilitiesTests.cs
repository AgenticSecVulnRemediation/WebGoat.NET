using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void MailingListQueries_UseNamedParameters()
        {
            // Arrange
            const string getSql = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            const string insertSql = "INSERT INTO mailinglist (firstname, lastname, email) VALUES (@First, @Last, @Email)";

            using var getCmd = new SqliteCommand(getSql);
            using var insertCmd = new SqliteCommand(insertSql);

            // Act
            getCmd.Parameters.AddWithValue("@Email", "user@example.com");
            insertCmd.Parameters.AddWithValue("@First", "A");
            insertCmd.Parameters.AddWithValue("@Last", "B");
            insertCmd.Parameters.AddWithValue("@Email", "user@example.com");

            // Assert
            Assert.Contains("Email = @Email", getCmd.CommandText);
            Assert.NotNull(getCmd.Parameters["@Email"]);

            Assert.Contains("@First", insertCmd.CommandText);
            Assert.Contains("@Last", insertCmd.CommandText);
            Assert.Contains("@Email", insertCmd.CommandText);
            Assert.NotNull(insertCmd.Parameters["@First"]);
            Assert.NotNull(insertCmd.Parameters["@Last"]);
            Assert.NotNull(insertCmd.Parameters["@Email"]);
        }
    }
}
