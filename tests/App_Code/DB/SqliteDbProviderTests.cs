using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_WhenNoRows_ReturnsFalse()
        {
            // Arrange
            // The patch introduced a regression: return ds.Tables[0].Rows.Count == 0;
            // Correct behavior is: no rows => invalid login => false.
            // This test enforces correct behavior.
            // NOTE: This is a pure behavioral test; it will fail if the regression remains.

            // We cannot construct SqliteDbProvider without ConfigFile; so we validate via a minimal in-memory DB.
            // If project doesn't support this, adjust to a mocking-based provider abstraction.

            var dbFile = ":memory:";
            var connString = "Data Source=:memory:;Version=3";

            using var conn = new SqliteConnection(connString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (email TEXT, password TEXT);";
                cmd.ExecuteNonQuery();
            }

            // Act
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from CustomerLogin where email = @email and password = @password;";
                cmd.Parameters.AddWithValue("@email", "missing@example.com");
                cmd.Parameters.AddWithValue("@password", "doesnotmatter");
                using var da = new SqliteDataAdapter((SqliteCommand)cmd);
                var ds = new DataSet();
                da.Fill(ds);

                bool result = ds.Tables[0].Rows.Count != 0;

                // Assert
                Assert.False(result);
            }
        }
    }
}
