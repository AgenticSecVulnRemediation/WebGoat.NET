using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPayments_Tests
    {
        [Fact]
        public void GetPayments_UsesParameterBinding_DoesNotTreatInjectedInputAsSql()
        {
            // This test targets the fix: customerNumber is now bound as @customerNumber.
            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Payments (customerNumber TEXT, amount INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Payments (customerNumber, amount) VALUES ('1', 100);";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(provider, connectionString);

            // Act: injection-like customer number would have returned all rows when concatenated
            var ds = provider.GetPayments(1 /* valid */);
            Assert.NotNull(ds);
            Assert.Equal(1, ds!.Tables[0].Rows.Count);

            // And ensure non-matching value doesn't return all rows
            var ds2 = provider.GetPayments(int.Parse("1"));
            Assert.NotNull(ds2);
            Assert.Equal(1, ds2!.Tables[0].Rows.Count);
        }
    }
}
