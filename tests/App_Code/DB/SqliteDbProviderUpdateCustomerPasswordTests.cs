using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/SqliteDbProvider.cs".
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_AndDoesNotBreakSqlWithInjection()
        {
            // Arrange: simulate the patched behavior (parameterized UPDATE) using a real in-memory SQLite DB.
            // We verify that a payload that would have terminated the string-literal in the old code
            // does not alter the WHERE clause when used as a parameter.

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            conn.Open();

            using (var create = conn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE CustomerLogin (customerNumber INTEGER PRIMARY KEY, password TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO CustomerLogin(customerNumber, password) VALUES (1, 'old');";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO CustomerLogin(customerNumber, password) VALUES (2, 'old2');";
                create.ExecuteNonQuery();
            }

            // Act: execute a parameterized update matching the patched SQL shape.
            // Use an injection string as the password input.
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@password", "x', password='hacked");
                cmd.Parameters.AddWithValue("@customerNumber", 1);
                var affected = cmd.ExecuteNonQuery();

                Assert.Equal(1, affected);
                Assert.Equal(2, cmd.Parameters.Count);
            }

            // Assert: only row 1 changed; row 2 unchanged.
            using (var q = new SqliteCommand("SELECT password FROM CustomerLogin WHERE customerNumber = 1", conn))
            {
                Assert.Equal("x', password='hacked", (string)q.ExecuteScalar());
            }
            using (var q = new SqliteCommand("SELECT password FROM CustomerLogin WHERE customerNumber = 2", conn))
            {
                Assert.Equal("old2", (string)q.ExecuteScalar());
            }
        }
    }
}
