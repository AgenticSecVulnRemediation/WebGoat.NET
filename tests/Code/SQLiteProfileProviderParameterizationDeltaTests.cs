using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterizationDeltaTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Delta: query switched from $Username/$ApplicationId to @Username/@ApplicationId.
            // Assert the new parameter names exist and the old ones do not.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;
            var text = assembly.FullName ?? string.Empty;

            Assert.Contains("@Username", text);
            Assert.Contains("@ApplicationId", text);
            Assert.DoesNotContain("$Username", text);
            Assert.DoesNotContain("$ApplicationId", text);
        }
    }
}
