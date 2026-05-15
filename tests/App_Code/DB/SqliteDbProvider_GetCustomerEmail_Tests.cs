using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmail_Tests
    {
        [Fact]
        public void GetCustomerEmail_NoRowReturned_ReturnsNull_InsteadOfThrowing()
        {
            // This test targets the fix: ExecuteScalar result is checked for null.
            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber INTEGER, email TEXT);";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(provider, connectionString);

            string result = provider.GetCustomerEmail("9999");

            Assert.Null(result);
        }

        [Fact]
        public void GetCustomerEmail_UsesParameterBinding_PreventsSqlInjection()
        {
            // Arrange
            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber TEXT, email TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO CustomerLogin (customerNumber, email) VALUES ('1', 'one@example.com');";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(provider, connectionString);

            // Act: injection-like value should not change query semantics when bound
            string injection = "1 OR 1=1";
            string result = provider.GetCustomerEmail(injection);

            // Assert: should not match, returning null
            Assert.Null(result);
        }
    }
}
