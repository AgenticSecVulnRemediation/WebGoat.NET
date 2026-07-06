using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterPrefixTests
    {
        [Fact]
        public void SQLiteProfileProvider_UsesAtParameterPrefix_ForLastActivityUpdate()
        {
            // This is a delta/regression test to ensure the provider uses the correct
            // SQLite parameter placeholder prefix for the affected UPDATE statement.
            // The security fix changed $LastActivityDate/$UserId to @LastActivityDate/@UserId.

            var src = GetProviderSource();

            Assert.Contains("SET LastActivityDate = @LastActivityDate", src);
            Assert.Contains("WHERE UserId = @UserId", src);
            Assert.DoesNotContain("LastActivityDate = $LastActivityDate", src);
            Assert.DoesNotContain("WHERE UserId = $UserId", src);
        }

        private static string GetProviderSource()
        {
            // In unit tests we don't parse/compile the full provider; this test asserts the
            // exact secure change in the shipped source to prevent regression.
            // The build/test pipeline should include the file content in the repository.
            return System.IO.File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");
        }
    }
}
