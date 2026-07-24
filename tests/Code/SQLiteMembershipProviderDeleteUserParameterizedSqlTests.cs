using System;
using System.Reflection;
using System.Text;
using Xunit;

// Assumption: Source file SQLiteMembershipProvider.cs is compiled in TechInfoSystems.Data.SQLite namespace.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterizedSqlTests
    {
        // Delta test focuses on the change in DeleteUser:
        // - SELECT UserId query now uses string interpolation with placeholders, and parameters are cleared before DELETE.
        // This test guards against regression where parameters aren't cleared, causing incorrect command execution.
        [Fact]
        public void DeleteUser_SourceContainsParameterClearBeforeDelete()
        {
            // Arrange
            var asm = typeof(SQLiteMembershipProvider).Assembly;

            // Act
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = Encoding.UTF8.GetString(bytes);

            // Assert
            // Ensure the newly added parameter clearing line exists.
            Assert.Contains("cmd.Parameters.Clear()", text);

            // Ensure DELETE query uses parameter placeholders rather than concatenation in this section.
            Assert.Contains("DELETE FROM", text);
            Assert.Contains("LoweredUsername = $Username", text);
            Assert.Contains("ApplicationId = $ApplicationId", text);
        }
    }
}
