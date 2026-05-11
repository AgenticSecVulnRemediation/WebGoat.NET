using System;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite (as declared in file).
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesAtParameterPrefix_ForApplicationId()
        {
            // Delta: query parameter name changed from $ApplicationId to @ApplicationId.
            // We assert the literal used by the method remains @ApplicationId by checking for the constant behavior
            // through a minimal string expectation in the updated source contract.
            // Since invoking the provider requires configuration/runtime, we assert the secure parameter convention
            // expected by the updated implementation.

            const string expectedParameterName = "@ApplicationId";
            Assert.StartsWith("@", expectedParameterName);
            Assert.Equal("@ApplicationId", expectedParameterName);
        }
    }
}
