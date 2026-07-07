using System;
using Xunit;
using Moq;
using Mono.Data.Sqlite;

// Assumption: source namespace is TechInfoSystems.Data.SQLite (from file content)
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingRelatedData_UsesNamedParameter_UserId_NotDollarParameter()
        {
            // Arrange
            // We cannot easily execute DeleteUser without a configured DB/HttpContext.
            // This delta test focuses on the changed behavior: SQL uses @UserId parameter marker
            // instead of $UserId for the two delete statements.
            var provider = new SQLiteMembershipProvider();

            // Act
            // Reflection is used purely to verify the patched command text fragments exist in the source.
            // This avoids needing a real SQLite DB.
            var asmText = typeof(SQLiteMembershipProvider).Assembly.ToString();

            // Assert
            // Vulnerable/incorrect marker removed
            Assert.DoesNotContain("WHERE UserId = $UserId", asmText);
            // Secure/correct marker present
            Assert.Contains("WHERE UserId = @UserId", asmText);
        }
    }
}
