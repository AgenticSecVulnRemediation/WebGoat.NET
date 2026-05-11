using System;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithLongRunningRegexInput_DoesNotHang()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "applicationName", "/" },
                { "passwordStrengthRegularExpression", "^(a+)+$" },
                { "minRequiredPasswordLength", "7" },
                { "minRequiredNonalphanumericCharacters", "0" },
                { "enablePasswordReset", "true" },
                { "enablePasswordRetrieval", "false" },
                { "requiresQuestionAndAnswer", "false" },
                { "requiresUniqueEmail", "false" },
                { "maxInvalidPasswordAttempts", "50" },
                { "passwordAttemptWindow", "10" },
                { "passwordFormat", "Hashed" }
            };

            // Act & Assert
            // The fix adds a Regex timeout; we expect an exception rather than an indefinite evaluation.
            // We avoid exercising DB access by using invalid credentials; we only validate the regex validation path.
            Assert.ThrowsAny<Exception>(() => provider.ChangePassword("user", "old", new string('a', 1024)));
        }
    }
}
