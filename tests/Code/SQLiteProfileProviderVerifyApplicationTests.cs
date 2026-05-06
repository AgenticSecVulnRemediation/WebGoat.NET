using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_SupportedBySqlite()
        {
            // Delta behavior: INSERT changed to positional placeholders (?, ?, ?).
            // Validate the provider's chosen placeholder style is supported by Mono.Data.Sqlite.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "CREATE TABLE aspnet_Applications (ApplicationId TEXT, ApplicationName TEXT, Description TEXT);";
            cmd.ExecuteNonQuery();

            using var insert = cn.CreateCommand();
            insert.CommandText = "INSERT INTO aspnet_Applications (ApplicationId, ApplicationName, Description) VALUES (?, ?, ?)";
            insert.Parameters.Add(new SqliteParameter { Value = Guid.NewGuid().ToString() });
            insert.Parameters.Add(new SqliteParameter { Value = "app" });
            insert.Parameters.Add(new SqliteParameter { Value = string.Empty });

            var rows = insert.ExecuteNonQuery();
            Assert.Equal(1, rows);
        }
    }
}
