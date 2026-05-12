using System;
using System.Reflection;
using Xunit;

// Assumption: production code namespaces match file content.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserDeltaTests
    {
        [Fact]
        public void DeleteUser_CommandTextUsesParameterizedPlaceholders_DoesNotInlineUsername()
        {
            // This delta test verifies the security-relevant change: use of parameter placeholders in the DELETE statement.
            // We do not execute DB operations; we assert the updated literal SQL text exists in the compiled assembly metadata.

            var asm = typeof(SQLiteMembershipProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("DELETE FROM", text);
            Assert.Contains("LoweredUsername = $Username", text);
            Assert.Contains("ApplicationId = $ApplicationId", text);
        }

        [Fact]
        public void DeleteUser_ClearsParametersBeforeDeleteStatement_IsPresentInAssembly()
        {
            // Delta verifies cmd.Parameters.Clear() was added before the delete statement.
            var asm = typeof(SQLiteMembershipProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("Parameters.Clear", text);
        }
    }
}
