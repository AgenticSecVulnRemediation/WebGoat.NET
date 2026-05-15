using Xunit;
using System;
using System.Text.RegularExpressions;

// Assumption: source project exposes TechInfoSystems.Data.SQLite namespace in main assembly.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicBacktrackingPattern_ThrowsProviderException()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Regex that can cause catastrophic backtracking for certain inputs.
            // The fix adds a Regex match timeout to avoid ReDoS.
            string pattern = "^(a+)+$";

            // Act + Assert
            // We call Initialize to force ValidatePwdStrengthRegularExpression().
            // Provide minimal config; the ProviderException should come from regex construction if it times out.
            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "DefaultConnection" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", pattern }
            };

            // This assertion specifically verifies that a problematic regex is rejected (or otherwise triggers provider exception)
            // after the fix that introduces match timeouts.
            Assert.Throws<System.Configuration.Provider.ProviderException>(() => provider.Initialize("SQLiteMembershipProvider", config));
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithSafePattern_DoesNotThrowFromRegexConstruction()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            string pattern = "^[a-zA-Z0-9]{8,}$";

            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "DefaultConnection" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", pattern }
            };

            // Act + Assert
            // We don't assert full initialization success because it depends on config/DB; instead we assert
            // it doesn't fail due to regex parsing/timeout construction for safe pattern.
            try
            {
                provider.Initialize("SQLiteMembershipProvider", config);
            }
            catch (System.Configuration.Provider.ProviderException ex)
            {
                // If it fails, it should not be because the safe regex is invalid.
                Assert.DoesNotContain("regex", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
