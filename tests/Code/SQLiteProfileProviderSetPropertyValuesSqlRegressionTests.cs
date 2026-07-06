using System;
using System.IO;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesSqlRegressionTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "Code", "SQLiteProfileProvider.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/Code/SQLiteProfileProvider.cs'.");
        }

        [Fact]
        public void SetPropertyValues_UsesPositionalParameters_ForCountAndUpsertAndEnforcesOrderingComments()
        {
            // Delta assertion: SELECT COUNT, UPDATE, INSERT now use positional placeholders (?) instead of named $ parameters.
            // This mitigates SQL injection and ensures correct parameter ordering.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "Code", "SQLiteProfileProvider.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("SELECT COUNT(*) FROM \" + PROFILE_TB_NAME + \" WHERE UserId = ?", text);
            Assert.Contains("cmd.Parameters.AddWithValue (null, userId)", text);

            Assert.Contains("UPDATE \" + PROFILE_TB_NAME + \" SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?", text);
            Assert.Contains("INSERT INTO \" + PROFILE_TB_NAME + \" (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)", text);

            // Ensure the conditional ordering block exists (prevents mismatched parameter ordering)
            Assert.Contains("if (cmd.CommandText.StartsWith(\"UPDATE\"))", text);
        }
    }
}
