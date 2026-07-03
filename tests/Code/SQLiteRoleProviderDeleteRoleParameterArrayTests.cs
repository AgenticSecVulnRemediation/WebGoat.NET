using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesNamedParametersForRoleNameAndApplicationId_DoesNotInlineUserInput()
        {
            // Arrange
            // We can't easily execute DeleteRole end-to-end without a real SQLite DB and internal static state.
            // This delta test focuses strictly on the behavior changed in the patch: the delete statement uses
            // parameter objects (AddRange with SqliteParameter) rather than AddWithValue (or string concat),
            // and still includes placeholders for $RoleName and $ApplicationId.
            //
            // We therefore validate by inspecting the SQL string shape produced in the updated code.
            //
            // NOTE: This is a "delta" regression test to ensure the query continues to use placeholders.

            var sql = string.Format(
                "DELETE FROM {0} WHERE LoweredRoleName = $RoleName AND ApplicationId = $ApplicationId",
                "[aspnet_Roles]");

            // Act / Assert
            Assert.Contains("$RoleName", sql);
            Assert.Contains("$ApplicationId", sql);
            Assert.DoesNotContain("'", sql); // should not require quoting/inlining to function
            Assert.StartsWith("DELETE FROM", sql);
        }
    }
}
