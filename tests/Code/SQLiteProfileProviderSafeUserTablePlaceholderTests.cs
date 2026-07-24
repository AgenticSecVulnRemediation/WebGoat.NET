using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_UnsafeSafeUserTablePlaceholderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_ShouldNotUseSafeUserTablePlaceholder()
        {
            // This is a regression guard: the patch introduced a placeholder table name ("SafeUserTable").
            // We assert the implementation does not contain that placeholder.
            // If it does, this test will fail and force correction.

            var source = System.IO.File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            Assert.DoesNotContain("SafeUserTable", source);
        }
    }
}
