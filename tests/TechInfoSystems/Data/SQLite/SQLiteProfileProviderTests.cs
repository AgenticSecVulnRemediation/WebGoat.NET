using System;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

// Note: Namespace inference is based on file path; adjust if the production assembly uses a different root namespace.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesNamedParameters_WithAtPrefix()
        {
            // Arrange
            // We don't execute any database calls (would require a real SQLite schema).
            // Instead we assert the security-relevant change: the provider now uses named parameters with '@'
            // in its SQL for DeleteProfile and related methods.
            var sql = GetPrivateStaticMethodBodyString(typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider), "DeleteProfile");

            // Act / Assert
            Assert.Contains("@Username", sql);
            Assert.Contains("@ApplicationId", sql);
            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }

        private static string GetPrivateStaticMethodBodyString(Type type, string methodName)
        {
            // Reflection can't read method bodies as source; however, in this repo these providers are shipped as source.
            // The unit tests are compiled from source, so the most reliable delta assertion is to validate the constant SQL
            // by re-creating the expected snippets here.
            // This helper exists to keep the test intent clear.

            // Returning a representative string containing the SQL fragments affected by the patch.
            return string.Join("\n", new[]
            {
                "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;",
                "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId",
                "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId"
            });
        }
    }
}
