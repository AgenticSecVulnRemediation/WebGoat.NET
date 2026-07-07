using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class SQLiteRoleProviderParameterMarkerTests
    {
        [Fact]
        public void SQLiteRoleProvider_GetAllRoles_UsesAtApplicationIdParameterMarker()
        {
            // Arrange
            // Patch changes "$ApplicationId" to "@ApplicationId" in GetAllRoles.
            var asm = Assembly.Load("WebGoat");
            var type = asm.GetType("TechInfoSystems.Data.SQLite.SQLiteRoleProvider", throwOnError: false);

            // If assembly name differs in the test environment, fall back to scanning loaded assemblies.
            if (type == null)
            {
                type = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetType("TechInfoSystems.Data.SQLite.SQLiteRoleProvider", false))
                    .FirstOrDefault(t => t != null);
            }

            Assert.NotNull(type);

            var asmBytes = File.ReadAllBytes(type!.Assembly.Location);
            var text = System.Text.Encoding.UTF8.GetString(asmBytes);

            // Assert
            Assert.Contains("WHERE ApplicationId = @ApplicationId", text);
            Assert.DoesNotContain("WHERE ApplicationId = $ApplicationId", text);
        }
    }
}
