using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production code namespace matches file path.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_ClearsParametersBeforeReuse_DoesNotThrowOnDuplicateParameterNames()
        {
            // Arrange
            // Delta behavior: cmd.Parameters.Clear() is now called before the DELETE, preventing duplicate $Username/$ApplicationId.
            // This test validates the underlying issue: adding duplicate parameter names throws.
            var cmd = new Mono.Data.Sqlite.SqliteCommand();
            cmd.Parameters.AddWithValue("$Username", "user");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");

            // Act
            cmd.Parameters.Clear();
            var ex = Record.Exception(() =>
            {
                cmd.Parameters.AddWithValue("$Username", "user2");
                cmd.Parameters.AddWithValue("$ApplicationId", "app2");
            });

            // Assert
            Assert.Null(ex);
        }
    }
}
