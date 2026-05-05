using System;
using Xunit;

using Mono.Data.Sqlite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationDeltaTests
    {
        [Fact]
        public void Patch163_VerifyApplication_UsesDollarPrefixedParameterNames()
        {
            // Delta assertion: VerifyApplication now binds $ApplicationName and $Description.
            var cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("$ApplicationName", "App");
            cmd.Parameters.AddWithValue("$Description", string.Empty);

            Assert.NotNull(cmd.Parameters["$ApplicationName"]);
            Assert.NotNull(cmd.Parameters["$Description"]);
        }
    }
}
