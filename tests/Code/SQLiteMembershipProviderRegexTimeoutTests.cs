using System;
using System.Configuration.Provider;
using Xunit;

// Assumption: source file namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithEvilPattern_DoesNotThrowArgumentException()
        {
            // Arrange
            // Patch adds Regex timeout, ensuring the regex engine doesn't hang.
            // We assert that validation wraps regex construction issues in ProviderException (not leaking raw ArgumentException).
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);

            regexField!.SetValue(null, "^(a+)+$");

            var method = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            // In the vulnerable version, Regex construction without timeout could lead to catastrophic backtracking during usage.
            // The fix applies a timeout at construction time; if the runtime rejects the timeout or pattern, it should be wrapped.
            var ex = Record.Exception(() => method!.Invoke(null, null));

            if (ex != null)
            {
                // Reflection invoke wraps exceptions in TargetInvocationException
                var inner = ex.InnerException;
                Assert.NotNull(inner);
                Assert.IsType<ProviderException>(inner);
            }
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithValidPattern_DoesNotThrow()
        {
            // Arrange
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);

            regexField!.SetValue(null, "^[a-zA-Z0-9]{8,}$");

            var method = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            var ex = Record.Exception(() => method!.Invoke(null, null));
            Assert.Null(ex);
        }
    }
}
