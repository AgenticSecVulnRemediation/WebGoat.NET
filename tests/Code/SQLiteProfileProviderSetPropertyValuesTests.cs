using System;
using System.Configuration;
using System.Web.Profile;
using Xunit;

// Assumption: production code namespace matches file path.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtStyleParametersForUserLookup()
        {
            // Arrange
            // Delta behavior: SQL and parameter names changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // Validate the expected placeholders.
            var expectedSql = "WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";

            // Act / Assert
            Assert.Contains("@Username", expectedSql);
            Assert.Contains("@ApplicationId", expectedSql);
            Assert.DoesNotContain("$Username", expectedSql);
        }
    }
}
