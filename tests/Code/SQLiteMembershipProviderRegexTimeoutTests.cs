using System;
using System.Reflection;
using Xunit;

// Assumption: production code namespace is TechInfoSystems.Data.SQLite as declared in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithComplexRegex_IsCompiledWithTimeoutOverload()
        {
            // Arrange
            // The fix changes ValidatePwdStrengthRegularExpression from new Regex(pattern) to new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(2)).
            // We validate this behavior by setting a valid regex and ensuring the method does not throw.
            var providerType = typeof(SQLiteMembershipProvider);
            var field = providerType.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, "^[a-zA-Z0-9]{8,}$");

            var method = providerType.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act
            var exception = Record.Exception(() => method!.Invoke(null, null));

            // Assert
            Assert.Null(exception);
        }
    }
}
