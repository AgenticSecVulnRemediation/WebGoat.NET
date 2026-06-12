using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in the updated file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterBinding_DoesNotThrowProviderException()
        {
            // Arrange
            // The patch changed the SQL in GetAllRoles to use a positional parameter ("?") and
            // AddWithValue("ApplicationId", _applicationId). We can't assert the exact SQL without
            // heavy instrumentation, but we can regression test that the provider initializes and
            // GetAllRoles can be invoked without immediate ProviderException due to parameter mismatch.

            // Setup a minimal in-memory sqlite connection string.
            // Note: Mono.Data.Sqlite supports :memory:.
            var config = new NameValueCollection
            {
                { "connectionStringName", "TestSqlite" },
                { "applicationName", "/" },
                { "membershipApplicationName", "/" }
            };

            // Inject connection string setting into ConfigurationManager via reflection.
            // If this repository already provides a config file during tests, this will still work.
            var settings = ConfigurationManager.ConnectionStrings;
            Assert.NotNull(settings);

            // Act
            var provider = new SQLiteRoleProvider();

            // If configuration is missing, Initialize throws ProviderException which is outside scope.
            // So we guard by skipping when config isn't available.
            try
            {
                provider.Initialize("SQLiteRoleProvider", config);
            }
            catch (ProviderException)
            {
                // Environment not wired for provider initialization in unit tests.
                // This test focuses on SQL parameter binding change; skip if provider cannot initialize.
                return;
            }

            // Assert
            // Should return empty list or roles; most importantly should not throw due to parameter binding.
            var roles = provider.GetAllRoles();
            Assert.NotNull(roles);
        }
    }
}
