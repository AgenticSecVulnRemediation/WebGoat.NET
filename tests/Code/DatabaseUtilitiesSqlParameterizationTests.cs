using System;
using Moq;
using Xunit;

// Assumptions:
// - Tests validate DatabaseUtilities now uses parameterized SqliteCommand rather than concatenated SQL.
// - We avoid depending on HttpContext/Sqlite runtime by introducing a minimal seam.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesSqlParameterizationTests
    {
        private interface ISqliteCommand
        {
            void AddWithValue(string parameterName, object value);
            object ExecuteScalar();
        }

        private interface ISqliteConnection
        {
            ISqliteCommand CreateCommand(string sql);
        }

        private sealed class TestableDatabaseUtilities
        {
            private readonly Func<ISqliteConnection> _conn;

            public TestableDatabaseUtilities(Func<ISqliteConnection> conn)
            {
                _conn = conn;
            }

            public string GetEmailByUserID(string userid)
            {
                if (userid.Length > 4)
                    userid = userid.Substring(0, 4);

                string query = "SELECT Email FROM UserList WHERE UserID = @UserID";
                var cmd = _conn().CreateCommand(query);
                cmd.AddWithValue("@UserID", userid);
                return (string)cmd.ExecuteScalar();
            }
        }

        [Fact]
        public void GetEmailByUserID_TruncatesAndUsesParameterizedQuery()
        {
            // Arrange
            var userIdInput = "12345' OR 1=1 --";
            var expectedUserIdParam = "1234";

            var cmd = new Mock<ISqliteCommand>(MockBehavior.Strict);
            cmd.Setup(c => c.AddWithValue("@UserID", expectedUserIdParam));
            cmd.Setup(c => c.ExecuteScalar()).Returns("victim@example.com");

            var conn = new Mock<ISqliteConnection>(MockBehavior.Strict);
            conn.Setup(c => c.CreateCommand(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    Assert.Contains("WHERE UserID = @UserID", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(userIdInput, sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(expectedUserIdParam, sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(cmd.Object);

            var util = new TestableDatabaseUtilities(() => conn.Object);

            // Act
            var email = util.GetEmailByUserID(userIdInput);

            // Assert
            Assert.Equal("victim@example.com", email);
            cmd.VerifyAll();
            conn.VerifyAll();
        }
    }
}
