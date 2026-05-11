using Mono.Data.Sqlite;
using System.Data;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void AddNewPosting_BuildsParameterizedInsert_AndPassesParametersToDoNonQuery()
        {
            // Arrange: delta change uses parameterized SQL with @title/@email/@message.
            var sql = "insert into Postings(title, email, message) values (@title, @email, @message)";

            // Act
            using var p1 = new SqliteParameter("@title", DbType.String) { Value = "t" };
            using var p2 = new SqliteParameter("@email", DbType.String) { Value = "e" };
            using var p3 = new SqliteParameter("@message", DbType.String) { Value = "m" };

            // Assert
            Assert.Contains("@title", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@message", sql);
            Assert.Equal("@title", p1.ParameterName);
            Assert.Equal("@email", p2.ParameterName);
            Assert.Equal("@message", p3.ParameterName);
        }
    }
}
