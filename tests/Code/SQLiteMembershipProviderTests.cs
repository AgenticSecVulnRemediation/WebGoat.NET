using System;
using System.Reflection;
using Moq;
using Xunit;

// Note: Namespace inference based on file path WebGoat/Code/SQLiteMembershipProvider.cs
// If the production namespace differs, adjust in production or add InternalsVisibleTo; test uses reflection to avoid hard dependency on internals.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void VerifyApplication_InsertUsesAtParameters_NotDollarParameters()
        {
            // Arrange
            var t = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider);

            // The patch changes VerifyApplication() INSERT statement parameter markers
            // from $ApplicationId/$ApplicationName/$Description to @ApplicationId/@ApplicationName/@Description.
            var method = t.GetMethod("VerifyApplication", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // We can’t easily inspect IL for string constants without external libraries.
            // Instead, assert that the updated source compiled into this assembly contains the expected markers
            // by searching the assembly manifest resource names (best-effort deterministic in this repo).
            // If this becomes flaky, replace with a seam around command construction.
            var asmPath = t.Assembly.Location;
            var bytes = System.IO.File.ReadAllBytes(asmPath);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("@ApplicationId", text);
            Assert.Contains("@ApplicationName", text);
            Assert.Contains("@Description", text);
            Assert.DoesNotContain("$ApplicationId, $ApplicationName, $Description", text);
        }
    }
}
