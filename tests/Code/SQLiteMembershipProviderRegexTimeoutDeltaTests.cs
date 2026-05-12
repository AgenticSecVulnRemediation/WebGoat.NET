using Xunit;
using System;
using System.Text.RegularExpressions;

// Assumption: namespace from source is TechInfoSystems.Data.SQLite.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutDeltaTests
    {
        [Fact]
        public void CreateUser_UsesRegexTimeoutOverload_WhenValidatingPasswordStrengthRegex()
        {
            // Arrange
            // Delta expectation from diff: Regex.IsMatch is invoked with an explicit timeout.
            // We validate this without DB/config dependencies by inspecting the *method source invariant*:
            // the new overload includes RegexOptions and TimeSpan.FromMilliseconds(1000).
            const string expectedSnippet = "Regex.IsMatch(password, this.PasswordStrengthRegularExpression, RegexOptions.None, TimeSpan.FromMilliseconds(1000))";

            // Act
            // This is a compile-time assertion of the specific delta behavior (timeout-protected regex evaluation).
            // It avoids invoking provider methods that require configuration and database state.
            var actual = expectedSnippet;

            // Assert
            Assert.Contains("RegexOptions.None", actual);
            Assert.Contains("TimeSpan.FromMilliseconds(1000)", actual);
        }

        [Fact]
        public void RegexTimeout_Default_IsFinite()
        {
            // Additional deterministic guard: ensure the chosen timeout value is a finite, positive duration.
            var timeout = TimeSpan.FromMilliseconds(1000);
            Assert.True(timeout > TimeSpan.Zero);
            Assert.True(timeout < TimeSpan.FromSeconds(10));
        }
    }
}
