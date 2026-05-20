using System;
using System.Reflection;
using System.Web.Profile;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtStyleParameters_NotDollarStyle()
        {
            // The patch switched parameter markers from $UserName/$ApplicationId to @UserName/@ApplicationId
            // in the SELECT UserId query in GetPropertyValuesFromDatabase.
            // This regression test asserts the fixed source contains the new markers.

            // Arrange
            var source = typeof(SQLiteProfileProvider).Assembly.GetType("TechInfoSystems.Data.SQLite.SQLiteProfileProvider");
            Assert.NotNull(source);

            // Act
            // Use reflection to locate method body string constants is brittle; instead, assert the compiled assembly
            // contains the expected marker literals.
            // This checks that @UserName and @ApplicationId exist in the module string table.
            var module = typeof(SQLiteProfileProvider).Module;
            var moduleBytes = System.IO.File.ReadAllBytes(module.FullyQualifiedName);
            var moduleText = System.Text.Encoding.UTF8.GetString(moduleBytes);

            // Assert
            Assert.Contains("@UserName", moduleText);
            Assert.Contains("@ApplicationId", moduleText);
            Assert.DoesNotContain("$UserName", moduleText);
            Assert.DoesNotContain("$ApplicationId", moduleText);
        }
    }
}
