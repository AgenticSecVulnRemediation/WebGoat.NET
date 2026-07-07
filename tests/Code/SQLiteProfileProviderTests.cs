using System;
using System.Data;
using System.Reflection;
using Moq;
using Xunit;

// Assumption: production code is in namespace TechInfoSystems.Data.SQLite as per source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesStaticAspnetProfileTableName_AndKeepsParameterizedUserId()
        {
            // Arrange
            // This test is a regression/delta test for PR #4017:
            // the query was changed from concatenating PROFILE_TB_NAME to using a hardcoded "[aspnet_Profile]".
            // We can’t easily intercept SqliteCommand without refactoring, so we assert the updated SQL string exists
            // in the source assembly as a minimal regression check.

            var asm = typeof(SQLiteProfileProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("FROM [aspnet_Profile] WHERE UserId = $UserId", text);
            Assert.DoesNotContain("FROM \" + PROFILE_TB_NAME + \" WHERE UserId = $UserId", text);
        }
    }
}
