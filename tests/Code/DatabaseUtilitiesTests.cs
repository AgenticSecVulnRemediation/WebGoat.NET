using Xunit;
using Mono.Data.Sqlite;
using System;
using System.Data;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        // Delta test: GetEmailByUserID now uses parameterized SqliteCommand.
        // We validate it can retrieve an email for a userId that contains quotes without breaking SQL.
        [Fact]
        public void GetEmailByUserID_WithSpecialCharacters_DoesNotBreakQueryAndReturnsNotFoundMessage()
        {
            // Arrange
            // Create instance without running ctor (relies on HttpContext). We'll inject a connection.
            var du = (DatabaseUtilities)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(DatabaseUtilities));

            string dbPath = System.IO.Path.GetTempFileName();
            string connectionString = $"Data Source={dbPath};Version=3";

            try
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE UserList (UserID TEXT, Email TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO UserList(UserID, Email) VALUES ('abcd', 'a@b.com');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Set private fields
                typeof(DatabaseUtilities).GetField("conn", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(du, new SqliteConnection(connectionString));

                // userId with quote that would have broken concatenated SQL; method truncates to 4 chars.
                string userId = "ab'cdef";

                // Act
                string email = du.GetEmailByUserID(userId);

                // Assert
                // Truncated to first 4 => "ab'c" which is not present; should return not found message, not throw.
                Assert.Contains("not found", email, StringComparison.OrdinalIgnoreCase);
            }
            finally
            {
                try { System.IO.File.Delete(dbPath); } catch { }
            }
        }
    }
}
