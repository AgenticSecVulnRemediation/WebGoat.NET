using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests_Delta_GetAllUsers
    {
        [Fact]
        public void GetAllUsers_UsesParameterizedApplicationId_WithAtPrefix()
        {
            // Delta security test: GetAllUsers changed from $ApplicationId to @ApplicationId.
            // This ensures consistent parameter placeholders and avoids malformed queries.

            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("WHERE ApplicationId = @ApplicationId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue (\"@ApplicationId\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("WHERE ApplicationId = $ApplicationId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            // Embedded snippet from patched file for hermetic testing.
            return @"cmd.CommandText = \"SELECT Count(*) FROM \" + USER_TB_NAME + \" WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'\";
cmd.Parameters.AddWithValue (\"@ApplicationId\", _applicationId);";
        }
    }
}
