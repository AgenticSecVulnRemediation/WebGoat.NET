using System;
using System.Reflection;
using Xunit;
using Moq;

// Assumption: production code namespace matches file namespace.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_RegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // The fix adds a 1 second timeout to Regex instantiation used to validate the configured regex.
            // We force the code path to run by setting the private static field and invoking the private method via reflection.
            var providerType = typeof(SQLiteMembershipProvider);

            // A known catastrophic-backtracking pattern when evaluated against long 'a' strings.
            // Note: the timeout is on Regex construction in the fixed code.
            string evilRegex = "^(a+)+$";

            var regexField = providerType.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);
            regexField!.SetValue(null, evilRegex);

            var method = providerType.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            // In the fixed version, creating the Regex with a timeout will throw RegexMatchTimeoutException
            // for some problematic patterns (implementation dependent). At minimum it must not hang indefinitely.
            // We assert the specific secure behavior: a timeout exception is surfaced (wrapped or direct).
            var ex = Record.Exception(() => method!.Invoke(null, null));

            // Reflection invocation wraps exceptions in TargetInvocationException.
            if (ex is TargetInvocationException tie && tie.InnerException != null)
                ex = tie.InnerException;

            Assert.IsType<RegexMatchTimeoutException>(ex);
        }
    }
}
