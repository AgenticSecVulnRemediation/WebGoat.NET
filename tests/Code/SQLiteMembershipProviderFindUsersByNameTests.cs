using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameTests
    {
        [Fact]
        public void FindUsersByName_WithCommasInSearch_ThrowsArgumentException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            // Initialize provider with minimal config required for method validation.
            // We intentionally do not provide a real DB; we expect parameter validation to fail before DB usage.
            provider.Initialize("SQLiteMembershipProvider", new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "applicationName", "TestApp" }
            });

            // Act + Assert
            // Secure behavior: prevent comma injection-like patterns in usernameToMatch.
            Assert.Throws<ArgumentException>(() => provider.FindUsersByName("a,b", 0, 10, out _));
        }
    }
}
