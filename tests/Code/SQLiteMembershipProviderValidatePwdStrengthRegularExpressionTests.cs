using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderValidatePwdStrengthRegularExpressionTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_ConfiguredWithTimeout_DoesNotThrow()
        {
            // Arrange
            // The security fix wraps the configured regex with a timeout via new Regex(pattern, options, timeout).
            // We validate that the method accepts a valid regex pattern and does not throw.
            SetPrivateStaticField("_passwordStrengthRegularExpression", "^a+$");

            // Act
            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "ValidatePwdStrengthRegularExpression",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(method);
            var ex = Record.Exception(() => method!.Invoke(null, null));
            Assert.Null(ex);
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_InvalidRegex_ThrowsProviderException()
        {
            // Arrange
            SetPrivateStaticField("_passwordStrengthRegularExpression", "(");

            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "ValidatePwdStrengthRegularExpression",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Act
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(null, null));

            // Assert
            // Implementation catches ArgumentException from Regex ctor and wraps in ProviderException.
            Assert.IsType<System.Configuration.Provider.ProviderException>(ex.InnerException);
        }

        private static void SetPrivateStaticField(string fieldName, string value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }
    }
}
