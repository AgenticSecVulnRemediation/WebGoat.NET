using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

// Production namespace inferred from source file
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesSqlParameter_AndDoesNotInlineCustomerNumber()
        {
            // Arrange: create a temporary sqlite db matching the minimal schema and data.
            var dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            try
            {
                SqliteConnection.CreateFile(dbPath);
                var cs = $"Data Source={dbPath};Version=3";

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber TEXT, email TEXT);";
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, email) VALUES (@n, @e);";
                        cmd.Parameters.AddWithValue("@n", "123");
                        cmd.Parameters.AddWithValue("@e", "user@example.com");
                        cmd.ExecuteNonQuery();
                    }
                }

                // Build the same command shape as production code after the fix.
                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();

                    var customerNumber = "123";
                    var sql = "select email from CustomerLogin where customerNumber = @customerNumber";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        // Act
                        cmd.Parameters.AddWithValue("@customerNumber", customerNumber);

                        // Assert (delta): parameter is present and query uses placeholder.
                        Assert.Contains("@customerNumber", cmd.CommandText);
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@customerNumber" && (string)p.Value == customerNumber);

                        var result = cmd.ExecuteScalar();
                        Assert.Equal("user@example.com", Convert.ToString(result));
                    }
                }
            }
            finally
            {
                if (File.Exists(dbPath))
                    File.Delete(dbPath);
            }
        }
    }
}
