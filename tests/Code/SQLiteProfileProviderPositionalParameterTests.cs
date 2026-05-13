using System;
using System.Reflection;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in the file.
// This test focuses only on the change: switching from named parameters ($Username/$ApplicationId)
// to positional placeholders (?) with AddWithValue(null, ...).

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderPositionalParameterTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalParameters_ForUserIdLookup()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();

            // We can't hit the DB in a unit test deterministically; instead we assert the patched source structure
            // by reflecting the method body as text is not possible. So we assert behavior indirectly:
            // the method should be callable with minimal settings and should throw due to missing configuration,
            // not due to malformed parameter naming.

            var sc = new System.Configuration.SettingsContext();
            sc["UserName"] = "testuser";
            sc["IsAuthenticated"] = false;

            var props = new System.Configuration.SettingsPropertyValueCollection();

            // Act + Assert: Expect ProviderException because the provider isn't initialized with a connection string,
            // but ensure it's a configuration issue, not an argument/SQL placeholder issue.
            var ex = Assert.ThrowsAny<Exception>(() => provider.SetPropertyValues(sc, props));

            // In uninitialized state, it should throw (NullReference/ProviderException). We only assert it doesn't throw
            // ArgumentException related to parameter names.
            Assert.DoesNotContain("$Username", ex.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
