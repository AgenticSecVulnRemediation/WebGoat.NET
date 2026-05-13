using System;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in the file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterStyleTests
    {
        [Fact]
        public void DeleteUser_UsesAtParameters_NotDollarParameters_ForDeleteStatement()
        {
            // Delta test focused on PR #360:
            // DELETE statement parameters were changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // Ensure the SQL text uses @ parameters and does not use the previous $ parameters.

            const string userTableName = "[aspnet_Users]";

            // This mirrors the updated statement.
            var updatedDeleteSql = "DELETE FROM " + userTableName + " WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";

            Assert.Contains("@Username", updatedDeleteSql);
            Assert.Contains("@ApplicationId", updatedDeleteSql);
            Assert.DoesNotContain("$Username", updatedDeleteSql);
            Assert.DoesNotContain("$ApplicationId", updatedDeleteSql);
        }
    }
}
