using System;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_ForPasswordAndCustomerNumber()
        {
            // Arrange
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
                        cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber INTEGER PRIMARY KEY, password TEXT);";
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, password) VALUES (1, 'old');";
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();

                    var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        // Act
                        cmd.Parameters.AddWithValue("@password", "new");
                        cmd.Parameters.AddWithValue("@customerNumber", 1);
                        var affected = cmd.ExecuteNonQuery();

                        // Assert (delta): command uses placeholders and parameters.
                        Assert.Equal(1, affected);
                        Assert.Contains("@password", cmd.CommandText);
                        Assert.Contains("@customerNumber", cmd.CommandText);
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@password");
                        Assert.Contains(cmd.Parameters, p => p.ParameterName == "@customerNumber");
                    }

                    using (var verify = conn.CreateCommand())
                    {
                        verify.CommandText = "SELECT password FROM CustomerLogin WHERE customerNumber = 1;";
                        var pwd = Convert.ToString(verify.ExecuteScalar());
                        Assert.Equal("new", pwd);
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
