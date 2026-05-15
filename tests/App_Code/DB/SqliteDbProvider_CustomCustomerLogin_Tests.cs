using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_CustomCustomerLogin_Tests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery_PreventsSqlInjection()
        {
            // This test targets the security fix: email is now bound as @Email instead of concatenated.
            // Arrange: create in-memory SQLite schema
            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT, Password TEXT);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO CustomerLogin (Email, Password) VALUES ('victim@example.com', 'ENC');";
                cmd.ExecuteNonQuery();
            }

            // Create provider instance without running its constructor (avoids file-based DB setup)
            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SqliteDbProvider));

            // Inject connection string
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(provider, connectionString);

            // Act: attempt classic injection that would have returned rows when concatenated
            string injectionEmail = "victim@example.com' OR 1=1 --";
            string error = provider.CustomCustomerLogin(injectionEmail, "anything");

            // Assert: parameterized query should not match any row => Email Address Not Found!
            Assert.Equal("Email Address Not Found!", error);
        }
    }
}
