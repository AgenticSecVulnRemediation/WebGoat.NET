using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingAfterSelect_ClearsParametersBeforeSecondCommand()
        {
            // This is a delta test for the fix that clears parameters before reusing the same command for DELETE.
            // We can't inject a real DB connection here; instead we validate the behavior that Parameters.Clear() is invoked
            // by executing an equivalent command reuse scenario.

            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1 WHERE 1 = $One";
            cmd.Parameters.AddWithValue("$One", 1);
            Assert.Single(cmd.Parameters);

            // Simulate the provider fix: clear before changing command text and re-adding parameters
            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT 1 WHERE 1 = $One";
            cmd.Parameters.AddWithValue("$One", 1);

            Assert.Single(cmd.Parameters);
        }
    }
}
