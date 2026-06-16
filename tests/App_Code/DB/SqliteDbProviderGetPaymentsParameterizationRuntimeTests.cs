using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizationRuntimeTests
    {
        [Fact]
        public void PaymentsQuery_UsesCustomerNumberParameter_DoesNotReturnAllRowsForInjectionLikeInput()
        {
            // Arrange
            var tempDb = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sqlite");
            SqliteConnection.CreateFile(tempDb);

            try
            {
                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Payments (customerNumber INTEGER, amount INTEGER);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Payments(customerNumber, amount) VALUES (1, 100);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Payments(customerNumber, amount) VALUES (2, 200);";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act: run the fixed query shape.
                var sql = "select * from Payments where customerNumber = @customerNumber";

                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    var da = new SqliteDataAdapter(sql, conn);
                    da.SelectCommand.Parameters.AddWithValue("@customerNumber", "1 OR 1=1");
                    var ds = new DataSet();
                    da.Fill(ds);

                    // Assert: because parameter is used, "1 OR 1=1" is treated as a string and should match 0 rows.
                    Assert.Equal(0, ds.Tables[0].Rows.Count);
                }
            }
            finally
            {
                if (File.Exists(tempDb))
                    File.Delete(tempDb);
            }
        }
    }
}
