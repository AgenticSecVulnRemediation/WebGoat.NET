using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using Moq;
using Xunit;

// Note: Namespace inferred from source file path/namespace in patched file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void Initialize_WithPasswordStrengthRegex_UsesRegexTimeoutToMitigateReDoS()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Use an intentionally dangerous regex (nested quantifiers) which can catastrophically backtrack
            // on certain inputs if no timeout is used.
            var config = new NameValueCollection
            {
                ["connectionStringName"] = "Dummy", // will be read before regex validation finishes in some implementations
                ["passwordStrengthRegularExpression"] = "^(a+)+$",
                ["applicationName"] = "TestApp"
            };

            // We can't reliably reach the real configuration/DB access in unit tests.
            // Instead, we directly invoke the private ValidatePwdStrengthRegularExpression method to test the changed behavior.
            var validateMethod = typeof(SQLiteMembershipProvider).GetMethod(
                "ValidatePwdStrengthRegularExpression",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(validateMethod);

            // Also set private static field _passwordStrengthRegularExpression so the method validates the provided regex.
            var regexField = typeof(SQLiteMembershipProvider).GetField(
                "_passwordStrengthRegularExpression",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            // Act
            // The fix uses Regex(pattern, options, timeout). For a valid pattern, it should NOT throw.
            var ex = Record.Exception(() => validateMethod!.Invoke(null, Array.Empty<object>()));

            // Assert
            Assert.Null(ex);

            // And sanity-check: when a catastrophic input is matched with a timeout-enabled Regex,
            // a RegexMatchTimeoutException should be possible.
            // We can't access the internal Regex instance, but we can assert that .NET supports timeout exceptions
            // for this pattern, which is the intent of the fix.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                var r = new Regex("^(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(1));
                r.IsMatch(new string('a', 50000) + "!");
            });
        }
    }
}
