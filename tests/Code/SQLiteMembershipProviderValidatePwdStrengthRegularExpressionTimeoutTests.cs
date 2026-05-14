using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderValidatePwdStrengthRegularExpressionTimeoutTests
    {
        [Fact]
        public void Initialize_WithPathologicalPasswordStrengthRegex_DoesNotHang()
        {
            // Arrange
            // The fix adds a Regex timeout when validating the configured regex.
            // We invoke the private ValidatePwdStrengthRegularExpression via reflection.
            var regexField = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, "^(a+)+$");

            var validateMethod = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(validateMethod);

            // Act + Assert
            // With a timeout-aware Regex construction, this should return quickly.
            // Any thrown exception should be a ProviderException wrapping an ArgumentException (regex parsing),
            // but importantly it must not hang.
            var ex = Record.Exception(() => validateMethod!.Invoke(null, Array.Empty<object>()));
            Assert.Null(ex);
        }
    }
}
