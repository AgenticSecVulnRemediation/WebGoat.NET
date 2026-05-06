using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameter_RecognizedBySqlite()
        {
            // Delta behavior: parameter placeholder changed from $RoleName to @RoleName.
            // Validate @-prefixed parameter name is accepted by Mono.Data.Sqlite.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1 WHERE @RoleName IS NOT NULL";
            cmd.Parameters.AddWithValue("@RoleName", "admin");

            var result = cmd.ExecuteScalar();
            Assert.Equal(1L, result);
        }
    }
}
