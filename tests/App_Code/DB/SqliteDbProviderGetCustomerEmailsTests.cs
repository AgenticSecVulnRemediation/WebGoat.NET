using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameter_AppendsPercentSafely()
        {
            // Arrange
            var connectionString = "Data Source=:memory:;Version=3";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin (Email) VALUES ('alice@example.com');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin (Email) VALUES ('bob@example.com');";
                cmd.ExecuteNonQuery();
            }

            var sql = "select email from CustomerLogin where email like @email";
            using var da = new SqliteDataAdapter(sql, conn);

            // Act
            da.SelectCommand.Parameters.AddWithValue("@email", "ali" + "%");
            var ds = new DataSet();
            da.Fill(ds);

            // Assert
            Assert.Equal(1, ds.Tables[0].Rows.Count);
            Assert.Equal("alice@example.com", ds.Tables[0].Rows[0][0]);
        }
    }
}
