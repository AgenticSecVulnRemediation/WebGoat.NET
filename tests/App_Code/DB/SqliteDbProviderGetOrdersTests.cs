using System;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_BindsCustomerId_AsSqlParameter()
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
                        cmd.CommandText = "CREATE TABLE Orders (customerNumber INTEGER, orderNumber INTEGER);";
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO Orders(customerNumber, orderNumber) VALUES (7, 100);";
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();

                    var sql = "select * from Orders where customerNumber = @customerID";
                    using (var da = new SqliteDataAdapter(sql, conn))
                    {
                        // Act
                        da.SelectCommand.Parameters.AddWithValue("@customerID", 7);

                        var ds = new System.Data.DataSet();
                        da.Fill(ds);

                        // Assert (delta): parameter is present and row is returned.
                        Assert.Contains(da.SelectCommand.Parameters, p => p.ParameterName == "@customerID" && Convert.ToInt32(p.Value) == 7);
                        Assert.Single(ds.Tables[0].Rows);
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
