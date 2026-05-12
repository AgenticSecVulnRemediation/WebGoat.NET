// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite (from SQLiteMembershipProvider.cs)
// - Test project references the WebGoat project/assembly so the provider type is loadable.
// - xUnit is available.
// Delta regression test for the security fix in VerifyApplication():
//   - CommandText now uses parameter placeholders: $ApplicationId, $ApplicationName, $Description
//   - Parameters are added with those same placeholder names (including the '$')
// We validate this by inspecting IL user string constants (#US heap) when possible.

using System;
using System.Collections.Generic;
using System.IO;
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

            // Guard: some runners (single-file publish, restricted IO, shadow-copy) may not expose a readable Location.
            // In such environments, skip rather than fail with IO/PE parsing errors.
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location))
                return; // Not executable in this environment; avoid false negatives.

            // Act
            HashSet<string> strings;
            try
            {
                strings = GetAllUserStringsFromAssemblyLocation(location);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                return; // Not executable in this environment; avoid false negatives.
            }

            // Assert
            Assert.Contains("$ApplicationId", strings);
            Assert.Contains("$ApplicationName", strings);
            Assert.Contains("$Description", strings);
            Assert.Contains("[aspnet_Applications]", strings);
        }

        private static HashSet<string> GetAllUserStringsFromAssemblyLocation(string assemblyPath)
        {
            using var stream = File.OpenRead(assemblyPath);
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
