using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Assumption: test project references the WebGoat assembly containing TechInfoSystems.Data.SQLite.
    public class SQLiteProfileProvider_VerifyApplicationParameterMarkersTests
    {
        [Fact]
        public void VerifyApplication_UsesAtParameterMarkers_InInsertStatement()
        {
            // This is a delta test that guards the security fix in PR #421:
            // VerifyApplication() switched from $param markers to @param markers.
            // We assert the fixed source uses the expected markers to avoid accidental regression.

            var source = GetEmbeddedSource();

            Assert.Contains("VALUES (@ApplicationId, @ApplicationName, @Description)", source);
            Assert.Contains("AddWithValue (\"@ApplicationId\"", source);
            Assert.Contains("AddWithValue (\"@ApplicationName\"", source);
            Assert.Contains("AddWithValue (\"@Description\"", source);

            // Previously it used $ApplicationId etc. Ensure those are not present in the insert statement.
            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", source);
        }

        private static string GetEmbeddedSource()
        {
            // The workflow provides updated file content via patch data; in-repo unit tests cannot access that directly.
            // We instead assert against the current working tree file in the PR.
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteProfileProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
