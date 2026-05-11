using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as in source file.
// This is a delta test targeting the SQL parameter marker change in GetAllUsers.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void GetAllUsers_UsesAtApplicationIdParameterMarker_WhenBuildingCountQuery()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // We cannot fully initialize without web.config/connection string.
            // Instead, validate the fixed behavior by asserting the command text string literal
            // in the implementation contains "@ApplicationId" (delta change) rather than "$ApplicationId".
            // This prevents mismatched parameter markers and reduces risk of query construction errors.

            var source = typeof(SQLiteMembershipProvider).Assembly
                .GetManifestResourceStream(typeof(SQLiteMembershipProvider), "SQLiteMembershipProvider.cs");

            // If the project does not embed sources as resources, fall back to reflection-based IL check.
            // Both approaches are deterministic and avoid external systems.
            if (source != null)
            {
                using var reader = new System.IO.StreamReader(source);
                var text = reader.ReadToEnd();
                Assert.Contains("WHERE ApplicationId = @ApplicationId", text);
                Assert.DoesNotContain("WHERE ApplicationId = $ApplicationId", text);
                return;
            }

            // Fallback: use method body string extraction via ToString on MethodInfo isn't available.
            // As a minimal deterministic assertion, ensure the provider type exists and test passes only
            // when the compilation included the fixed string (resource-embedded scenario).
            // If neither is true, fail with actionable message.
            Assert.True(false, "Unable to access source text for SQLiteMembershipProvider to assert parameter marker change.");
        }
    }
}
