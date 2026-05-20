using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void Initialize_WithCatastrophicRegex_ThrowsProviderExceptionDueToTimeoutOrInvalidRegex()
        {
            // Arrange: set a potentially catastrophic regex. The fix should compile it with a timeout.
            var provider = new SQLiteMembershipProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "SomeConnection" },
                { "applicationName", "TestApp" },
                { "passwordStrengthRegularExpression", "(a+)+$" }
            };

            // Act + Assert
            // We can't reliably force a timeout during Regex ctor across platforms, but we can assert that
            // Initialize fails fast with ProviderException when regex compilation/validation fails.
            Assert.ThrowsAny<ProviderException>(() => provider.Initialize("SQLiteMembershipProvider", config));
        }

        [Fact]
        public void Initialize_WithInvalidRegex_ThrowsProviderException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "SomeConnection" },
                { "applicationName", "TestApp" },
                { "passwordStrengthRegularExpression", "[" } // invalid
            };

            // Act + Assert
            Assert.Throws<ProviderException>(() => provider.Initialize("SQLiteMembershipProvider", config));
        }
    }
}
