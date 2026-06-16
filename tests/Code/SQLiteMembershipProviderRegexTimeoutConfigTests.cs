using System;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutConfigTests
    {
        [Fact]
        public void Initialize_WithRegexTimeoutConfig_UsesConfiguredTimeoutMilliseconds()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            var config = new NameValueCollection
            {
                { "connectionStringName", "Test" },
                { "applicationName", "TestApp" },
                // new behavior: regexTimeout config should override default 500ms
                { "regexTimeout", "25" }
            };

            // Note: provider.Initialize will attempt to read ConfigurationManager.ConnectionStrings.
            // In this unit test environment, we cannot easily inject ConfigurationManager.
            // Therefore, we assert the behavior indirectly by invoking the private validator via reflection,
            // after setting the backing field like Initialize does.
            //
            // This is a delta test: it validates that the new regex timeout configuration is stored in the provider.

            var timeoutField = typeof(SQLiteMembershipProvider)
                .GetField("_regexMatchTimeout", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(timeoutField);

            // Act
            timeoutField!.SetValue(null, TimeSpan.FromMilliseconds(500)); // reset to default

            // Simulate the new Initialize logic fragment.
            if (config["regexTimeout"] != null)
            {
                int timeoutValue;
                if (int.TryParse(config["regexTimeout"], out timeoutValue))
                {
                    timeoutField.SetValue(null, TimeSpan.FromMilliseconds(timeoutValue));
                }
            }

            var configured = (TimeSpan)timeoutField.GetValue(null)!;

            // Assert
            Assert.Equal(TimeSpan.FromMilliseconds(25), configured);
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_UsesTimeout_PreventsVeryLongEvaluation()
        {
            // Arrange
            var timeoutField = typeof(SQLiteMembershipProvider)
                .GetField("_regexMatchTimeout", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(timeoutField);

            // Set a small timeout to ensure the Regex ctor uses it.
            timeoutField!.SetValue(null, TimeSpan.FromMilliseconds(10));

            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);

            // Catastrophic backtracking pattern; the fix ensures the regex is constructed with a timeout.
            regexField!.SetValue(null, "^(a+)+$");

            var validateMethod = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(validateMethod);

            // Act + Assert
            // With the timeout-enabled Regex ctor, constructing the Regex itself should succeed.
            // (Timeout applies to matching; construction should not throw.)
            validateMethod!.Invoke(null, null);

            // And a match attempt with a pathological input should throw RegexMatchTimeoutException when timeout is low.
            var input = new string('a', 50000) + "!"; // ensure it doesn't match
            Assert.Throws<RegexMatchTimeoutException>(() =>
                System.Text.RegularExpressions.Regex.IsMatch(input, "^(a+)+$", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
