using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSqlParameterizationTests
    {
        [Fact]
        public void SqliteCommand_AllowsUsingAtNamedParameters_ForUsernameAndApplicationId()
        {
            // Delta behavior: query changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // Validate the provider's chosen parameter style is accepted by Mono.Data.Sqlite.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1 WHERE @Username IS NOT NULL AND @ApplicationId IS NOT NULL";
            cmd.Parameters.AddWithValue("@Username", "user");
            cmd.Parameters.AddWithValue("@ApplicationId", Guid.NewGuid().ToString());

            var result = cmd.ExecuteScalar();
            Assert.Equal(1L, result);
        }
    }
}
