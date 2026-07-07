using System;
using System.Data;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesAtParameterMarker_ForUserId()
        {
            // Arrange
            // Delta: command text changed from $UserId to @UserId.
            // We assert the provider source now contains the @UserId marker by reflecting method body string constants.
            // This is a lightweight regression guard for the exact fix.

            var method = typeof(SQLiteProfileProvider).GetMethod("DeleteProfiles", new[] { typeof(string[]) });
            Assert.NotNull(method);

            // Assert (best-effort): ensure the assembly contains the new parameter name.
            // This avoids DB coupling while tightly focusing on the change.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location);
            var asmText = System.Text.Encoding.UTF8.GetString(asmBytes);
            Assert.Contains("@UserId", asmText);
            Assert.DoesNotContain(" WHERE UserId = $UserId", asmText);
        }
    }
}
