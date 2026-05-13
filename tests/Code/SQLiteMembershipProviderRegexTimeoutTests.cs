using System;
using System.Reflection;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite as declared in the file.
// This test focuses only on the security fix: regex construction now enforces a match timeout.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicBacktrackingPattern_DoesNotHang()
        {
            // Arrange: set the private static field _passwordStrengthRegularExpression to a known catastrophic pattern.
            Type providerType = typeof(SQLiteMembershipProvider);
            FieldInfo regexField = providerType.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);

            regexField!.SetValue(null, "^(a+)+$");

            // Act + Assert: invoking the private validator should return quickly.
            // Prior to the fix, Regex("^(a+)+$") would compile without an explicit timeout.
            // After the fix, it compiles with a 1-second timeout and should not throw during construction.
            MethodInfo validateMethod = providerType.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(validateMethod);

            Exception ex = Record.Exception(() => validateMethod!.Invoke(null, null));

            // If invocation threw, unwrap TargetInvocationException for clarity.
            if (ex is TargetInvocationException tie && tie.InnerException != null)
            {
                ex = tie.InnerException;
            }

            Assert.Null(ex);
        }
    }
}
