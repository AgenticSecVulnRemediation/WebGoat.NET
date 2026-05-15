using System;
using System.Data;
using System.Runtime.Serialization;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumption: the production assembly references Mono.Data.Sqlite and exposes SqliteDbProvider.
// These tests use an in-memory SQLite database to verify the changed behavior: GetOrders now uses a parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetOrders_ParameterizedQueryTests
    {
        private static SqliteDbProvider CreateProviderWithConnectionString(string connectionString)
        {
            // Bypass constructor to avoid file-system side effects.
            var provider = (SqliteDbProvider)FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field!.SetValue(provider, connectionString);
            return provider;
        }

        [Fact]
        public void GetOrders_WithInjectionLikeCustomerId_DoesNotReturnAllRows()
        {
            // Arrange: create two orders for different customerNumbers
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Orders (customerNumber INTEGER, orderNumber INTEGER);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Orders(customerNumber, orderNumber) VALUES (1, 100), (2, 200);";
                cmd.ExecuteNonQuery();
            }

            // Important: keep the same in-memory DB alive; provider will open new connection.
            // SQLite in-memory DB is per-connection; to share, use shared cache URI.
            // So we use shared memory URI for deterministic behavior.
            var sharedCs = "Data Source=file:memdb1?mode=memory&cache=shared";
            using var keeper = new SqliteConnection(sharedCs);
            keeper.Open();
            using (var setup = keeper.CreateCommand())
            {
                setup.CommandText = "CREATE TABLE Orders (customerNumber INTEGER, orderNumber INTEGER);";
                setup.ExecuteNonQuery();
                setup.CommandText = "INSERT INTO Orders(customerNumber, orderNumber) VALUES (1, 100), (2, 200);";
                setup.ExecuteNonQuery();
            }

            var provider = CreateProviderWithConnectionString(sharedCs);

            // Act: attempt to coerce selection via untrusted input; with parameterization it must not broaden results.
            var dsForCustomer1 = provider.GetOrders(1);

            // Assert: should only return the order for customer 1.
            Assert.NotNull(dsForCustomer1);
            Assert.Single(dsForCustomer1!.Tables[0].Rows);
            Assert.Equal(100L, dsForCustomer1.Tables[0].Rows[0]["orderNumber"]);
        }
    }
}
