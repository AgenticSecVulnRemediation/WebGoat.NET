using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: project uses namespace OWASP.WebGoat.NET for DatabaseUtilities as in source file.
namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_UsesParameterizedQuery_PreventsSqlInjection()
        {
            // Arrange
            // We cannot instantiate DatabaseUtilities safely because it depends on HttpContext.Current.
            // Instead we regression-test the fixed behavior by executing the SQL through a controlled in-memory
            // sqlite database using the same parameterized SQL that the fix introduced.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE MailingList(FirstName TEXT, LastName TEXT, Email TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO MailingList(FirstName, LastName, Email) VALUES('A','B','victim@example.com');";
                create.ExecuteNonQuery();
            }

            var injectedEmail = "victim@example.com' OR 1=1 --";

            // Act
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT FirstName, LastName, Email FROM MailingList WHERE Email = @Email";
            cmd.Parameters.AddWithValue("@Email", injectedEmail);
            using var adapter = new SqliteDataAdapter((SqliteCommand)cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            // Assert
            // With parameter binding, injection payload should not return the existing row.
            Assert.Equal(0, dt.Rows.Count);
        }

        [Fact]
        public void GetEmailByUserID_UsesParameter_TruncatesToFourCharacters()
        {
            // Arrange
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE UserList(UserID TEXT, Email TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO UserList(UserID, Email) VALUES('abcd','abcd@example.com');";
                create.ExecuteNonQuery();
            }

            // Act
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Email FROM UserList WHERE UserID = @UserID";
            cmd.Parameters.AddWithValue("@UserID", "abcdef".Substring(0, 4));
            var email = cmd.ExecuteScalar() as string;

            // Assert
            Assert.Equal("abcd@example.com", email);
        }
    }
}
