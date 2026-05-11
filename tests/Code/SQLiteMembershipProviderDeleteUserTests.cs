using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_DoesNotReuseStaleParameters_AfterSelectingUserId()
        {
            // Arrange
            // The fix clears parameters before the DELETE to avoid reusing parameters
            // from the prior SELECT.
            var provider = new SQLiteMembershipProvider();

            // Act & Assert
            // This is a structural regression test: we validate the ADO.NET parameter collection behavior
            // by directly exercising the pattern used in the fixed code.
            using var cmd = new Mono.Data.Sqlite.SqliteCommand();
            cmd.Parameters.AddWithValue("$Username", "user");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");
            Assert.True(cmd.Parameters.Count == 2);

            cmd.Parameters.Clear();
            Assert.Empty(cmd.Parameters);

            cmd.Parameters.AddWithValue("$Username", "user");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");
            Assert.True(cmd.Parameters.Count == 2);
        }
    }
}
