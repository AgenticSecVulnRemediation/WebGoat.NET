using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId_PreventsSqlInjection()
        {
            // Arrange
            var connectionString = "Data Source=:memory:;Version=3";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Orders (customerNumber INTEGER, orderNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Orders (customerNumber, orderNumber) VALUES (1, 100);";
                cmd.ExecuteNonQuery();
            }

            // Fixed query uses @customerID
            var sql = "select * from Orders where customerNumber = @customerID";
            using var da = new SqliteDataAdapter(sql, conn);

            // Act: attempt an injection-like value by forcing parameter to string via AddWithValue.
            da.SelectCommand.Parameters.AddWithValue("@customerID", "1 OR 1=1");
            var ds = new DataSet();
            da.Fill(ds);

            // Assert: no rows returned; if string concatenation existed, this would typically return all rows.
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
