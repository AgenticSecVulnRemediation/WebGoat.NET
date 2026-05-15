using System;
using Xunit;

// Assumption: source file namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithEvilBacktrackingPattern_ThrowsProviderExceptionDueToRegexTimeout()
        {
            // Arrange
            // Pattern with catastrophic backtracking against the test string below.
            // The fix adds a timeout to Regex construction, which should result in a thrown exception being wrapped into ProviderException.
            var provider = new SQLiteMembershipProvider();

            // Use reflection to set the private static field _passwordStrengthRegularExpression
            var field = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, "^(a+)+$");

            // Act + Assert
            var method = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // ProviderException is expected as the implementation catches ArgumentException and wraps it.
            var ex = Assert.Throws<System.Configuration.Provider.ProviderException>(() => method!.Invoke(null, null));
            Assert.Contains("timeout", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithSimplePattern_DoesNotThrow()
        {
            // Arrange
            var field = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field!.SetValue(null, "^[a-zA-Z0-9]{8,}$");

            var method = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            var exception = Record.Exception(() => method!.Invoke(null, null));
            Assert.Null(exception);
        }
    }
}
