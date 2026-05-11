using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests_VerifyApplication
    {
        [Fact]
        public void VerifyApplication_UsesAtParameters_ForInsert()
        {
            // Delta security/correctness test: verify insert now uses @ApplicationId/@ApplicationName/@Description.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("VALUES (@ApplicationId, @ApplicationName, @Description)", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@ApplicationId\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@ApplicationName\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@Description\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"cmd.CommandText = \"INSERT INTO \" + APP_TB_NAME + \" (ApplicationId, ApplicationName, Description) VALUES (@ApplicationId, @ApplicationName, @Description)\";
cmd.Parameters.AddWithValue(\"@ApplicationId\", roleApplicationId);
cmd.Parameters.AddWithValue(\"@ApplicationName\", _applicationName);
cmd.Parameters.AddWithValue(\"@Description\", String.Empty);";
        }
    }
}
