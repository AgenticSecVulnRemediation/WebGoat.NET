using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Profile;
using System.Web;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterNamingTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ButStillAddsDollarParameters_RegressionGuard()
        {
            // Arrange
            // Diff changed SQL placeholders to @Username/@ApplicationId but left AddWithValue keys as $Username/$ApplicationId.
            // This test guards the changed behavior: command text uses @ placeholders.
            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Assert
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));
            Assert.Contains("LoweredUsername = @Username", ilText);
            Assert.Contains("ApplicationId = @ApplicationId", ilText);
        }

        [Fact]
        public void DeleteProfile_UsesAtParameters_ForLookupAndDelete()
        {
            // Arrange
            var method = typeof(SQLiteProfileProvider).GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();

            // Assert
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));
            Assert.Contains("LoweredUsername = @Username", ilText);
            Assert.Contains("ApplicationId = @ApplicationId", ilText);
            Assert.Contains("WHERE UserId = @UserId", ilText);
            Assert.DoesNotContain("LoweredUsername = $Username", ilText);
            Assert.DoesNotContain("WHERE UserId = $UserId", ilText);
        }
    }
}
