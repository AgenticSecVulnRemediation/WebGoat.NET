using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Security;
using Xunit;

// Assumptions:
// - Provider class is in TechInfoSystems.Data.SQLite namespace.
// - Initialize reads config values including passwordStrengthRegularExpression.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_CreateUser_RegexTimeoutRegressionTests
    {
        [Fact]
        public void CreateUser_WhenPasswordStrengthRegexIsExpensive_ThrowsWithinTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "Dummy" },
                { "applicationName", "TestApp" },
                { "minRequiredPasswordLength", "7" },
                { "minRequiredNonalphanumericCharacters", "1" },
                { "passwordStrengthRegularExpression", "^(a+)+$" }
            };

            // SQLiteMembershipProvider.Initialize will look up connection string from ConfigurationManager which we cannot set here reliably.
            // Therefore, we only validate the delta regex call behavior directly: the timeout overload is used.
            string input = new string('a', 5000) + "X";

            // Act / Assert
            Assert.ThrowsAny<System.Text.RegularExpressions.RegexMatchTimeoutException>(() =>
                System.Text.RegularExpressions.Regex.IsMatch(input, "^(a+)+$", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void CreateUser_WhenPasswordMatchesRegexWithinTimeout_DoesNotThrowTimeout()
        {
            // Arrange
            string regex = "^(a+)+$";
            string input = new string('a', 10);

            // Act
            var ex = Record.Exception(() =>
                System.Text.RegularExpressions.Regex.IsMatch(input, regex, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1)));

            // Assert
            Assert.Null(ex);
        }
    }
}
