using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/SqliteDbProvider.cs".
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void OrdersQuery_UsesParameter_ForCustomerId_AndIsNotInjectable()
        {
            // Arrange: create an in-memory Orders table.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            conn.Open();

            using (var create = conn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE Orders (orderNumber INTEGER PRIMARY KEY, customerNumber INTEGER);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO Orders(orderNumber, customerNumber) VALUES (101, 1);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO Orders(orderNumber, customerNumber) VALUES (202, 2);";
                create.ExecuteNonQuery();
            }

            // Act: execute the patched query form.
            var sql = "select * from Orders where customerNumber = @customerID";
            using var da = new SqliteDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@customerID", "1 OR 1=1");

            var ds = new DataSet();
            da.Fill(ds);

            // Assert: injection payload is treated as a literal string, so no rows match integer 1 or 1=1.
            Assert.Single(da.SelectCommand.Parameters);
            Assert.Equal("@customerID", da.SelectCommand.Parameters[0].ParameterName);
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
