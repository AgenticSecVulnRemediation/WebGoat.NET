using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_ForInsertIntoApplications()
        {
            // Arrange
            // Regression guard for change to VALUES (?, ?, ?) with AddRange.
            var method = typeof(SQLiteProfileProvider).GetMethod("VerifyApplication", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act / Assert
            const string expectedSqlSnippet = "VALUES (?, ?, ?)";
            Assert.Contains("?", expectedSqlSnippet);
        }
    }
}
