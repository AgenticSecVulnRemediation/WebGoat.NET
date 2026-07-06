using System;
using System.Collections.Specialized;
using System.Reflection;
using Moq;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtPrefixedParametersForUserLookup_DoesNotUseDollarPrefixedParameters()
        {
            // This is a delta test that locks in the security fix from PR #3982:
            // the initial user lookup in SetPropertyValues must use @Username/@ApplicationId.
            // We assert against the current source text to ensure the vulnerable "$Username/$ApplicationId" variant
            // is not reintroduced.

            var sourcePath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteProfileProvider.cs");
            var text = System.IO.File.ReadAllText(sourcePath);

            Assert.Contains("LoweredUsername = @Username", text);
            Assert.Contains("ApplicationId = @ApplicationId", text);

            Assert.DoesNotContain("LoweredUsername = $Username", text);
            Assert.DoesNotContain("ApplicationId = $ApplicationId;\"", text);
        }
    }
}
