// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite (from SQLiteMembershipProvider.cs)
// - Test project references the WebGoat project/assembly so the provider type is loadable.
// - xUnit is available.
// This is a delta regression test for the security fix in VerifyApplication():
//   - CommandText now uses parameter placeholders: $ApplicationId, $ApplicationName, $Description
//   - Parameters are added with the same placeholder names (including the '$')
// We validate this by inspecting IL string constants (no DB/network access).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationDeltaTests
    {
        [Fact]
        public void VerifyApplication_UsesParameterizedPlaceholdersForInsertCommand()
        {
            // Arrange
            var asm = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider).Assembly;

            // Act
            var strings = GetAllUserStringsFromAssembly(asm);

            // Assert
            // The fix changed the INSERT statement to use placeholders ($ApplicationId, $ApplicationName, $Description).
            Assert.Contains("$ApplicationId", strings);
            Assert.Contains("$ApplicationName", strings);
            Assert.Contains("$Description", strings);

            // Additionally assert the table constant is still referenced.
            Assert.Contains("[aspnet_Applications]", strings);
        }

        private static HashSet<string> GetAllUserStringsFromAssembly(Assembly assembly)
        {
            // Read #US (User String) heap from the PE metadata.
            // This is deterministic and does not require executing the provider.
            var location = assembly.Location;
            using var stream = System.IO.File.OpenRead(location);
            using var peReader = new System.Reflection.PortableExecutable.PEReader(stream);

            if (!peReader.HasMetadata)
                return new HashSet<string>(StringComparer.Ordinal);

            var mdReader = peReader.GetMetadataReader();
            var results = new HashSet<string>(StringComparer.Ordinal);

            foreach (var handle in mdReader.UserStrings)
            {
                try
                {
                    var s = mdReader.GetUserString(handle);
                    if (!string.IsNullOrEmpty(s))
                        results.Add(s);
                }
                catch
                {
                    // Ignore malformed entries; continue scanning.
                }
            }

            return results;
        }
    }
}
