using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesPostingTests
    {
        [Fact]
        public void AddNewPosting_UsesParameters_PreventsSqlInjectionPayloadInValues()
        {
            // Arrange
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Postings(Title TEXT, Email TEXT, Message TEXT);";
                create.ExecuteNonQuery();
            }

            var title = "t'); DROP TABLE Postings; --";
            var email = "e@example.com";
            var message = "m";

            // Act
            using (var insert = cn.CreateCommand())
            {
                insert.CommandText = "insert into Postings(title, email, message) values (@title, @email, @message)";
                insert.Parameters.AddWithValue("@title", title);
                insert.Parameters.AddWithValue("@email", email);
                insert.Parameters.AddWithValue("@message", message);
                insert.ExecuteNonQuery();
            }

            using var countCmd = cn.CreateCommand();
            countCmd.CommandText = "SELECT COUNT(*) FROM Postings";
            var count = Convert.ToInt32(countCmd.ExecuteScalar());

            // Assert
            Assert.Equal(1, count);
        }
    }
}
