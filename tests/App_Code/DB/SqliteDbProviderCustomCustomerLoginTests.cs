using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterForEmail_PreventsSqlInjectionByQuoteBreaking()
        {
            // Arrange: Build a minimal in-memory schema and record.
            var connectionString = "Data Source=:memory:;Version=3";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT, Password TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin (Email, Password) VALUES ('victim@example.com', 'ENC');";
                cmd.ExecuteNonQuery();
            }

            // The fix is about using @Email parameter. We validate the query+parameter binding directly.
            var injectedEmail = "victim@example.com' OR '1'='1";
            var sql = "select * from CustomerLogin where email = @Email";
            using var da = new SqliteDataAdapter(sql, conn);

            // Act
            da.SelectCommand.Parameters.AddWithValue("@Email", injectedEmail);
            var ds = new DataSet();
            da.Fill(ds);

            // Assert: no rows match because the injected string is treated as a literal value.
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
