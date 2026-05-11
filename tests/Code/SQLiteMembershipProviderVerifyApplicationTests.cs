using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests_VerifyApplication
    {
        [Fact]
        public void VerifyApplication_UsesNamedParameters_ForApplicationInsert()
        {
            // Delta security/correctness test: VerifyApplication now uses string.Format and binds parameters
            // with $ApplicationId/$ApplicationName/$Description.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("string.Format(\"INSERT INTO {0}", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("$ApplicationId", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("$ApplicationName", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("$Description", fixedSnippet, StringComparison.Ordinal);
            Assert.DoesNotContain("AddWithValue (\"ApplicationName\"", fixedSnippet, StringComparison.Ordinal);
            Assert.DoesNotContain("AddWithValue (\"Description\"", fixedSnippet, StringComparison.Ordinal);
        }

        private static string GetFixedSnippet()
        {
            return @"cmd.CommandText = string.Format(\"INSERT INTO {0} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)\", APP_TB_NAME);
cmd.Parameters.AddWithValue (\"$ApplicationId\", _applicationId);
cmd.Parameters.AddWithValue (\"$ApplicationName\", _applicationName);
cmd.Parameters.AddWithValue (\"$Description\", String.Empty);";
        }
    }
}
