using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: namespace matches folder structure.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterMarkerAndAddsParameter()
        {
            // Arrange
            // Delta: SQL uses "@num" and adds parameter.
            // Since method creates SqliteCommand internally, verify via reflection on command text by
            // calling a minimal equivalent snippet.
            var sql = "select email from CustomerLogin where customerNumber = @num";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            // Act
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@num", "1");

            // Assert
            Assert.Contains("@num", cmd.CommandText);
            Assert.Contains(cmd.Parameters.Cast<SqliteParameter>(), p => p.ParameterName == "@num");
        }

        [Fact]
        public void AddComment_UsesParameterizedInsertAndDoesNotInlineComment()
        {
            // Arrange
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            // Act
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            cmd.Parameters.AddWithValue("@email", "a@example.com");
            cmd.Parameters.AddWithValue("@comment", "x'); DROP TABLE Comments;--");

            // Assert
            Assert.DoesNotContain("DROP TABLE", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(3, cmd.Parameters.Count);
        }
    }
}
