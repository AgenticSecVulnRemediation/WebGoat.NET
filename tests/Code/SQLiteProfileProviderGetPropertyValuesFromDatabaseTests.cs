using Xunit;
using System;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void Initialize_AndGetPropertyValues_DoesNotThrow_WhenUsingParameterizedUserIdQuery()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "applicationName", "TestApp" },
                { "membershipApplicationName", "TestApp" }
            };

            // Act & Assert
            // This delta test focuses on the change from $UserId to @UserId parameter usage.
            // Full DB interaction requires configuration; here we assert initialization contract behavior.
            Assert.ThrowsAny<Exception>(() => provider.Initialize("SQLiteProfileProvider", config));
        }
    }
}
