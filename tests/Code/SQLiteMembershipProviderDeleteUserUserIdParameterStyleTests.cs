using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserUserIdParameterStyleTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingRelatedData_UsesAtUserIdParameterMarker()
        {
            // Arrange
            // Delta change in PR: the SQL for deleting related records switched from $UserId to @UserId.
            // We verify that the fixed source contains @UserId in the relevant DELETE statements.
            // Since the provider composes the command text at runtime and requires DB wiring,
            // we verify the behavior by scanning the IL's user string (ldstr) constants.
            var providerType = typeof(SQLiteMembershipProvider);
            var method = providerType.GetMethod("DeleteUser", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();
            var module = providerType.Module;
            bool foundAtUserId = false;

            for (int i = 0; i < ilBytes.Length - 4; i++)
            {
                // ldstr opcode = 0x72 followed by 4-byte metadata token
                if (ilBytes[i] != 0x72) continue;
                int token = BitConverter.ToInt32(ilBytes, i + 1);
                try
                {
                    string s = module.ResolveString(token);
                    if (s != null && s.Contains("@UserId", StringComparison.Ordinal))
                    {
                        foundAtUserId = true;
                        break;
                    }
                }
                catch
                {
                    // ignore invalid tokens
                }
            }

            // Assert
            Assert.True(foundAtUserId,
                "DeleteUser should use the @UserId parameter marker in related-data DELETE statements (regression guard for the fix)." );
        }
    }
}
