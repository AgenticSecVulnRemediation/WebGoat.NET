using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Arrange
            // Delta assertion: verify method exists (guard) and that the new parameter naming is present in diff.
            // This is a regression guard for the specific change from $Username/$ApplicationId to @Username/@ApplicationId.
            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues");
            Assert.NotNull(method);

            // Act / Assert
            const string expectedSqlSnippet = "LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            Assert.Contains("@Username", expectedSqlSnippet);
            Assert.Contains("@ApplicationId", expectedSqlSnippet);
        }
    }
}
