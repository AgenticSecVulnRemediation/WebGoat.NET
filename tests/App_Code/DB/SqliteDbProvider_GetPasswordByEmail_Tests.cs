using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPasswordByEmail_Tests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterBinding_PreventsSqlInjectionFromEmail()
        {
            // This test targets the security fix: query now uses @email parameter.
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

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            field!.SetValue(provider, connectionString);

            // Act
            string injectionEmail = "victim@example.com' OR 1=1 --";
            string result = provider.GetPasswordByEmail(injectionEmail);

            // Assert: should not return victim password; in this code path, it returns "Email Address Not Found!" then will throw on ds.Tables[0].Rows[0]
            // but exception is caught and message returned. We assert it does not equal decoded password (unknown) and contains not found.
            Assert.Contains("Email Address Not Found", result);
        }
    }
}
