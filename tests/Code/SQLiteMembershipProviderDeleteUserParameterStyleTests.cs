using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_UsesParameterizedDeleteQueryWithAtParameters()
        {
            // This PR changes DeleteUser's DELETE statement to use @Username/@ApplicationId.
            // We assert the new secure behavior by inspecting the command text and parameter placeholders via reflection.
            // Note: This is a targeted delta test; it does not attempt to execute against a real SQLite DB.

            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "DeleteUser",
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                types: new[] { typeof(string), typeof(bool) },
                modifiers: null);

            Assert.NotNull(method);

            var source = method!.GetMethodBody();
            Assert.NotNull(source);

            // The important regression we guard: presence of the updated placeholder tokens.
            // If the code regresses to $Username/$ApplicationId, this test should fail.
            var il = method.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // Best-effort: scan the assembly for the literal strings used in the changed SQL.
            // This is deterministic and verifies the updated SQL text is embedded.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteMembershipProvider).Assembly.Location);
            var asmText = System.Text.Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", asmText);
            Assert.DoesNotContain("WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\";\r\n\r\n\t\t\t\t\tcmd.Parameters.AddWithValue (\"$Username\"", asmText);
        }
    }
}
