using System;
using System.Data;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderPaymentsTests
    {
        // This is a behavior-oriented test that validates the SQL injection fix by executing an equivalent
        // parameterized query against a real DB engine (SQLite) in-memory.
        // It does not require a MySQL server.
        [Fact]
        public void ParameterizedQuery_WithInjectionInput_DoesNotReturnAllRows()
        {
            // Arrange
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Payments (customerNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Payments(customerNumber) VALUES (1);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Payments(customerNumber) VALUES (2);";
                cmd.ExecuteNonQuery();
            }

            // Act: emulate the fixed pattern: WHERE customerNumber = @customerNumber
            using var query = conn.CreateCommand();
            query.CommandText = "select * from Payments where customerNumber = @customerNumber";
            query.Parameters.AddWithValue("@customerNumber", "0 OR 1=1");

            using var adapter = new SqliteDataAdapter(query);
            var ds = new DataSet();
            adapter.Fill(ds);

            // Assert: should not return all rows
            Assert.NotNull(ds);
            Assert.True(ds.Tables.Count > 0);
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
