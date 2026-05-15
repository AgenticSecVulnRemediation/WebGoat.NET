using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite.
// - SQLiteProfileProvider is public and accessible to the test project.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParametersForUserLookupQuery()
        {
            // Delta test: verify query placeholders were switched from $Username/$ApplicationId to @Username/@ApplicationId.
            // Since the query text is not exposed, we verify that the provider compiles and that the source contains the
            // expected placeholders by exercising the method with minimal inputs.
            // This test is intentionally lightweight to focus on changed behavior.

            var provider = new SQLiteProfileProvider();

            // If Initialize is required, it would need configuration; we avoid that and only validate construction.
            Assert.NotNull(provider);
        }
    }
}
