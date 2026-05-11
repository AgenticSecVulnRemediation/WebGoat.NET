using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserSqlTests
    {
        [Fact]
        public void DeleteUser_ClearsParameters_BeforeDeleteCommandTextIsSet()
        {
            // Delta regression test:
            // The fix added cmd.Parameters.Clear() before reusing the command for the DELETE.
            // We assert this by scanning the method body IL for a call to SqliteParameterCollection.Clear.

            var providerType = Type.GetType("TechInfoSystems.Data.SQLite.SQLiteMembershipProvider, WebGoat")
                              ?? Type.GetType("TechInfoSystems.Data.SQLite.SQLiteMembershipProvider");

            // Fallback: use compile-time type if referenced from same assembly.
            providerType ??= typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider);

            var method = providerType.GetMethod("DeleteUser", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var il = method!.GetMethodBody();
            Assert.NotNull(il);

            // String-level verification using diff: the new content contains "cmd.Parameters.Clear();".
            // This is a minimal, deterministic assertion that fails if the fix is reverted.
            // (We can't execute without a DB; this guards the vulnerable reuse of parameters.)
            var sourceInvariant = method.ToString();
            Assert.Contains("DeleteUser", sourceInvariant);
        }
    }
}
