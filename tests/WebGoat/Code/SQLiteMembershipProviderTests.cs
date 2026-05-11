using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void Initialize_WithPasswordStrengthRegex_DoesNotThrow_WithTimeoutOverloadInUse()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Ensure HostingEnvironment access doesn't throw for ApplicationVirtualPath.
            // If it's not available in this test environment, provide explicit applicationName.
            var config = new NameValueCollection
            {
                ["connectionStringName"] = "Membership",
                ["applicationName"] = "TestApp",
                ["passwordStrengthRegularExpression"] = "^.{7,}$"
            };

            // Provide required connection string via ConfigurationManager (must exist in test runner config).
            // If not present in the repo test environment, this test will need app.config/web.config wiring.

            // Act
            var ex = Record.Exception(() => provider.Initialize("SQLiteMembershipProvider", config));

            // Assert
            Assert.Null(ex);
        }
    }
}
